using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MassTransit;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Para Keycloak
using BookingMS.Core.DataBase;
using BookingMS.Core.Repositories;
using BookingMS.Infrastructure.DataBase;
using BookingMS.Infrastructure.Repositories;
using BookingMS.Application.Handlers.Commands;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuración de Servicios (igual que UsersMS) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // (Tu configuración de Swagger con Bearer Token)
});
builder.Services.AddHttpClient();

// --- 2. Configuración de la BD (Postgres) ---
var dbConnectionString = builder.Configuration.GetValue<string>("DefaultConnection");
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseNpgsql(dbConnectionString));

// Inyectar interfaces de tu plantilla
builder.Services.AddTransient<IBookingDbContext, BookingDbContext>();
builder.Services.AddTransient<IBookingRepository, BookingRepository>();

// --- 3. Configuración de MediatR y Validators ---
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateBookingCommandHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(CreateBookingCommandHandler).Assembly);

// --- 4. Configuración de Autenticación (Keycloak) ---
// Configurar autenticación JWT con Keycloak si el Gateway lo requiere aun no se

// --- 5. Configuración de MassTransit (RabbitMQ + Outbox) ---
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddEntityFrameworkOutbox<BookingDbContext>(outboxConfig =>
    {
        outboxConfig.QueryDelay = TimeSpan.FromSeconds(10);
        outboxConfig.UseBusOutbox(c => c.ConcurrentMessageDelivery = true); 
    });

    busConfig.SetKebabCaseEndpointNameFormatter();

    busConfig.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });
        cfg.ConfigureEndpoints(context);
    });

    busConfig.AddConsumer<SeatLockedConsumer>();
    busConfig.AddConsumer<SeatReleasedConsumer>();
    busConfig.AddConsumer<PaymentCapturedConsumer>();
});

// --- 6. Configuración de Hangfire ---
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(dbConnectionString)));

builder.Services.AddHangfireServer();

// --- Construir la App ---
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- Middlewares de Seguridad y Jobs ---
// app.UseAuthentication(); // <-- Activar autenticación
// app.UseAuthorization();  // <-- Activar autorización

app.UseHangfireDashboard("/hangfire"); // Dashboard de Hangfire

app.MapControllers();
app.Run();