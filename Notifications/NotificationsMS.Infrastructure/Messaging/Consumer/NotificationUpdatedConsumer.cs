using MassTransit;
using NotificationsMS.Domain.Entities;
using NotificationsMS.Infrastructure.DataBase;
using NotificationsMS.Commons.Events;
using MongoDB.Driver;

namespace NotificationsMS.Infrastructure.Messaging.Consumers;

public class NotificationUpdatedConsumer : IConsumer<NotificationUpdateEvent>
{
    private readonly MongoDbContext _mongo;

    public NotificationUpdatedConsumer(MongoDbContext mongo)
    {
        _mongo = mongo;
    }

    public async Task Consume(ConsumeContext<NotificationUpdateEvent> context)
    {
        var message = context.Message;

        var filter = Builders<NotificationReadModel>.Filter.Eq("IdNotification", message.IdNotification);
        var update = Builders<NotificationReadModel>.Update.Set("State", message.State);

        await _mongo.Notifications.UpdateOneAsync(filter, update);
    }

}
