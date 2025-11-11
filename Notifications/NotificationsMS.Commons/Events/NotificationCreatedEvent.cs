using NotificationsMS.Domain.Entities;

namespace NotificationsMS.Commons.Events;

public class NotificationCreatedEvent
{
    public Guid IdNotification { get; set; }
    public Guid IdUser { get; set; }
    public string Message { get; set; } = default!;
    public NotificationState State { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
