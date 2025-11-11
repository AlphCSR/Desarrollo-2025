using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationsMS.Domain.Entities;

public class NotificationReadModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid IdNotification { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid IdUser { get; set; }
    public string Message { get; set; }
    public NotificationState State { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

