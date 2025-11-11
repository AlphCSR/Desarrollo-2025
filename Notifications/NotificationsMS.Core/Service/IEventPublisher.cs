using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Domain.Entities;

namespace NotificationsMS.Core.Service
{
    public interface IEventPublisher
    {
        Task PublishNotificationCreatedAsync(Notification notification);
        Task PublishNotificationUpdatedAsync(Notification notification);
    }
}
