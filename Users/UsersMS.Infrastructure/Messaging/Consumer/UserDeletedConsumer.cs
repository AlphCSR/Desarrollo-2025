using MassTransit;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Commons.Events;
using MongoDB.Driver;

namespace UsersMS.Infrastructure.Messaging.Consumers;

public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
{
    private readonly MongoDbContext _mongo;

    public UserDeletedConsumer(MongoDbContext mongo)
    {
        _mongo = mongo;
    }

    public async Task Consume(ConsumeContext<UserDeletedEvent> context)
    {
        var message = context.Message;
        var filter = Builders<UserReadModel>.Filter.Eq(u => u.Id, message.Id);
        await _mongo.Users.DeleteOneAsync(filter);
    }
}
