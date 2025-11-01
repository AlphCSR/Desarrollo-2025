using System;

namespace UsersMS.Commons.Events
{
    public class UserDeletedEvent
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public object Role { get; set; }
        public object State { get; set; }
    }
}
