using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Commons.Enums;

namespace UsersMS.Commons.Dtos.Request
{
    public record CreateUserDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? DocumentId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public UserRole Role { get; set; }
        public UserState State { get; set; }
    }
}
