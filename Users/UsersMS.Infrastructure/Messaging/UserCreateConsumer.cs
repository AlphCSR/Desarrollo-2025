using MassTransit;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Commons.Events;

namespace UsersMS.Infrastructure.Messaging.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly UsersDbContext _dbContext;

    public UserCreatedConsumer(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var message = context.Message;

        var user = new User
        {
            Id = message.Id,
            Email = message.Email,
            Name = message.Name,
            DocumentId = message.DocumentId,
            LastName = message.LastName,
            Phone = message.Phone,
            Address = message.Address,
            Password = message.Password,
            Role = message.Role,
            State = message.State
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}
