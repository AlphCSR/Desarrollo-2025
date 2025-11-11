using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Domain.Entities;

namespace NotificationsMS.Commons.Events
{
    public class NotificationUpdateEvent
    {
        public Guid IdNotification { get; set; }
        public Guid IdUser { get; set; }
        public string Message { get; set; } = string.Empty; 
        public NotificationState State { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
