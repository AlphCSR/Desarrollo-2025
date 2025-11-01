using MassTransit;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Commons.Events;

namespace UsersMS.Infrastructure.Messaging.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly MongoDbContext _mongo;

    public UserCreatedConsumer(MongoDbContext mongo)
    {
        _mongo = mongo;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var message = context.Message;

        var user = new UserReadModel
        {
            Id = message.Id,
            Email = message.Email,
            Name = message.Name,
            LastName = message.LastName,
            Phone = message.Phone,
            Address = message.Address,
            Password = message.Password,
            Role = message.Role,
            State = message.State
        };

        await _mongo.Users.InsertOneAsync(user);
    }

}
