using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Domain.Entities;
using NotificationsMS.Domain.ValueObjects;

namespace NotificationsMS.Commons.Dtos.Request
{
    public class CreateNotificationDto
    {
        public Guid IdNotification { get; set; }
        public Guid IdUser { get; set; }
        public string Message { get; set; } = default!;
        public string Email { get; set; } = default!;
        public NotificationState State { get; set; } = NotificationState.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
