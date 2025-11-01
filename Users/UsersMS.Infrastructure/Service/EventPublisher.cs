using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using UsersMS.Domain.Entities;
using UsersMS.Commons.Events;
using UsersMS.Core.Service;

namespace UsersMS.Infrastructure.Service
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishUserCreatedAsync(User user)
        {
            var @event = new UserCreatedEvent
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.Phone,
                Address = user.Address,
                Password = user.Password,
                Role = user.Role,
                State = user.State
            };

            await _publishEndpoint.Publish(@event);
        }
        public async Task PublishUserUpdatedAsync(User user)
        {
            var @event = new UserUpdatedEvent
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.Phone,
                Address = user.Address,
                Password = user.Password,
                Role = user.Role,
                State = user.State
            };

            await _publishEndpoint.Publish(@event);
        }

        public async Task PublishUserDeletedAsync(User user)
        {
            var @event = new UserDeletedEvent
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.Phone,
                Address = user.Address,
                Password = user.Password,
                Role = user.Role,
                State = user.State
            };

            await _publishEndpoint.Publish(@event);
        }
    }
}

