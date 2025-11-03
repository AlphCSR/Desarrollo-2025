using MassTransit;
using UsersMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Commons.Events;

namespace UsersMS.Infrastructure.Messaging.Consumers;

public class UserUpdatedConsumer : IConsumer<UserUpdatedEvent>
{
    private readonly UsersDbContext _dbContext;

    public UserUpdatedConsumer(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
    {
        var message = context.Message;

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == message.Id);

        if (user != null)
        {
            user.Email = message.Email;
            user.Name = message.Name;
            user.LastName = message.LastName;
            user.DocumentId = message.DocumentId;
            user.Phone = message.Phone;
            user.Address = message.Address;
            user.Password = message.Password; // Considerar si la contraseña debe estar en el modelo de lectura
            user.Role = message.Role;
            user.State = message.State;
            
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}