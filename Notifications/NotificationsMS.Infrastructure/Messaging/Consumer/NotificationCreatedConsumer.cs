using MassTransit;
using NotificationsMS.Domain.Entities;
using NotificationsMS.Infrastructure.DataBase;
using NotificationsMS.Commons.Events;
using MongoDB.Driver;

namespace NotificationsMS.Infrastructure.Messaging.Consumers;

public class NotificationCreatedConsumer : IConsumer<NotificationCreatedEvent>
{
    private readonly MongoDbContext _mongo;

    public NotificationCreatedConsumer(MongoDbContext mongo)
    {
        _mongo = mongo;
    }

    public async Task Consume(ConsumeContext<NotificationCreatedEvent> context)
    {
        var message = context.Message;

        var Notifications = new NotificationReadModel
        {
            IdNotification = message.IdNotification,
            IdUser = message.IdUser,
            Message = message.Message,
            State = message.State,
            CreatedAt = message.CreatedAt,
        };

        await _mongo.Notifications.InsertOneAsync(Notifications);
    }

}
