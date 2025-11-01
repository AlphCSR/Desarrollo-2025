using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UsersMS.Commons.Enums;

namespace UsersMS.Domain.Entities;

public class UserReadModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } // ← ya no string

    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Password { get; set; } = default!;

    [BsonRepresentation(BsonType.String)]
    public UserRole Role { get; set; }

    [BsonRepresentation(BsonType.String)]
    public UserState State { get; set; }
}

