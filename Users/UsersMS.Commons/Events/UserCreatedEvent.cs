using UsersMS.Commons.Enums;


namespace UsersMS.Commons.Events;

public class UserCreatedEvent
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Password { get; set; } = default!;
    public UserRole Role { get; set; }
    public UserState State { get; set; }
}


