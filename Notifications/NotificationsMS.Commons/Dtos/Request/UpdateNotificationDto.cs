using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Domain.Entities;
using NotificationsMS.Domain.ValueObjects;

namespace NotificationsMS.Commons.Dtos.Request
{
    public class UpdateNotificationDto
    {
        public Guid IdNotification { get; set; }
        public Guid IdUser { get; set; }
        public string Message { get; set; } = default!;
        public NotificationState State { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
