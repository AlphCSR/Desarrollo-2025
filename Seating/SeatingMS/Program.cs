using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MassTransit;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SeatingMS.Core.DataBase;
using SeatingMS.Core.Repositories;
using SeatingMS.Infrastructure.DataBase;
using SeatingMS.Infrastructure.Repositories;
using SeatingMS.Application.Handlers.Commands;
using SeatingMS.Infrastructure.Consumers; // <-- Importar Consumidores
using SeatingMS.Application.Jobs; // <-- Importar Jobs

var builder = WebApplication.CreateBuilder(args);

// ... (Configuración de Controllers, Swagger, Auth/Keycloak como en EventsMS) ...

// --- 2. Configuración de la BD (Postgres) ---
var dbConnectionString = builder.Configuration.GetValue<string>("DefaultConnection");
builder.Services.AddDbContext<SeatingDbContext>(options =>
    options.UseNpgsql(dbConnectionString));

builder.Services.AddTransient<ISeatingDbContext, SeatingDbContext>();
builder.Services.AddTransient<IEventSeatRepository, EventSeatRepository>();
builder.Services.AddTransient<IVenueRepository, VenueRepository>();
// ... (Configuración de MediatR y Validators) ...

// --- 5. Configuración de MassTransit (RabbitMQ + Outbox) ---
builder.Services.AddMassTransit(busConfig =>
{
    // --- REGISTRAR CONSUMIDORES ---
    busConfig.AddConsumer<EventCreatedConsumer>();
    busConfig.AddConsumer<BookingConfirmedConsumer>();
    busConfig.AddConsumer<BookingExpiredConsumer>();
    
    busConfig.AddEntityFrameworkOutbox<SeatingDbContext>(outboxConfig =>
    {
        outboxConfig.QueryDelay = TimeSpan.FromSeconds(10);
        outboxConfig.UseBusOutbox();
    });

    busConfig.SetKebabCaseEndpointNameFormatter();

    busConfig.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h => { /* ... */ });
        
        // Configura los endpoints para los consumidores registrados
        cfg.ConfigureEndpoints(context); 
    });
});

// --- 6. Configuración de Hangfire ---
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(dbConnectionString)));

builder.Services.AddHangfireServer();
// --- REGISTRAR JOB ---
builder.Services.AddTransient<ISeatExpirationJob, SeatExpirationJob>();

var app = builder.Build();

// ... (Configuración de App: Swagger, Auth, HangfireDashboard) ...


app.MapControllers();
app.Run();