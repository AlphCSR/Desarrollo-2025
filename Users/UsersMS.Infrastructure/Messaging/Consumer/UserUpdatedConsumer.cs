using MassTransit;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Commons.Events;
using MongoDB.Driver;

namespace UsersMS.Infrastructure.Messaging.Consumers;

public class UserUpdatedConsumer : IConsumer<UserUpdatedEvent>
{
    private readonly MongoDbContext _mongo;

    public UserUpdatedConsumer(MongoDbContext mongo)
    {
        _mongo = mongo;
    }

    public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
    {
        var message = context.Message;

        var filter = Builders<UserReadModel>.Filter.Eq(u => u.Id, message.Id);
        var update = Builders<UserReadModel>.Update
            .Set(u => u.Email, message.Email)
            .Set(u => u.Name, message.Name)
            .Set(u => u.LastName, message.LastName)
            .Set(u => u.Phone, message.Phone)
            .Set(u => u.Address, message.Address)
            .Set(u => u.Password, message.Password)
            .Set(u => u.Role, message.Role)
            .Set(u => u.State, message.State);

        await _mongo.Users.UpdateOneAsync(filter, update);
    }
}
