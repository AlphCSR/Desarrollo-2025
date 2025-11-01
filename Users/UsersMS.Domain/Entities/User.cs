using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Commons.Enums;
using UsersMS.Domain.Entities;

namespace UsersMS.Domain.Entities
{
    public class User : Base
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string DocumentId { get; set; } = default!; // ← cédula u otro ID visible
        public string Name { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Address { get; set; } = default!;
        public UserRole Role { get; set; }
        public UserState State { get; set; }

        public User() { }

        public User(string email, string password, string documentId, string name, string lastname, string phone, string address, UserRole role, UserState state)
        {
            Id = Guid.NewGuid(); // base.Id
            Email = email;
            Password = password;
            DocumentId = documentId;
            Name = name;
            LastName = lastname;
            Phone = phone;
            Address = address;
            Role = role;
            State = state;
            CreatedAt = DateTime.UtcNow;
        }
    }
}

