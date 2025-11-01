using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Commons.Dtos.Request;

namespace UsersMS.Application.Commands
{
    public class CreateUserCommand : IRequest<string>
    {
        public CreateUserDto CreateUserDto { get; set; }

        public CreateUserCommand(CreateUserDto dto)
        {
            CreateUserDto = dto;
        }
    }
}
