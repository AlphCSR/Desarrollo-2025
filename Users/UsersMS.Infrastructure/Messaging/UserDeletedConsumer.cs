using MassTransit;
using UsersMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Commons.Events;

namespace UsersMS.Infrastructure.Messaging.Consumers;

public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
{
    private readonly UsersDbContext _dbContext;

    public UserDeletedConsumer(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserDeletedEvent> context)
    {
        var message = context.Message;
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == message.Id);
        
        if (user != null)
        {
            user.State = Commons.Enums.UserState.Inactive;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}