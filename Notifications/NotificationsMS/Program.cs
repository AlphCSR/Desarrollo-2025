using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NotificationsMS.Application.Handlers.Commands;
using NotificationsMS.Core.DataBase;
using NotificationsMS.Core.Repositories;
using NotificationsMS.Core.Service;
using NotificationsMS.Core.Messaging.Sender;
using NotificationsMS.Infrastructure.DataBase;
using NotificationsMS.Infrastructure.Repositories;
using NotificationsMS.Infrastructure.Settings;
using System.Configuration;
using MassTransit;
using NotificationsMS.Infrastructure.Messaging.Consumers;
using NotificationsMS.Infrastructure.Service;
using NotificationsMS.Infrastructure.Messaging.Sender;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.SqlServer;
using MassTransit.RabbitMqTransport;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var _appSettings = new AppSettings();
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
_appSettings = appSettingsSection.Get<AppSettings>();
builder.Services.Configure<AppSettings>(appSettingsSection);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddHttpClient();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateNotificationCommandHandler).Assembly));

builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<IEmail, Email>();
builder.Services.AddTransient<INotificationsDbContext, NotificationDbContext>();
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();

var dbConnectionString = builder.Configuration.GetValue<string>("DefaultConnection");
builder.Services.AddDbContext<NotificationDbContext>(options =>
options.UseSqlServer(dbConnectionString));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton(provider =>
{
    var context = provider.GetRequiredService<MongoDbContext>();
    return context.Notifications;
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotificationCreatedConsumer>();
    x.AddConsumer<NotificationUpdatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("notification-created-queue", e =>
        {
            e.ConfigureConsumer<NotificationCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("notification-updated-queue", e =>
        {
            e.ConfigureConsumer<NotificationUpdatedConsumer>(context);
        });
    });
});


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();  
app.MapControllers();
app.Run();

