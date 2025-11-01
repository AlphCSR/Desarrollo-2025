using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Application.Queries;
using UsersMS.Commons.Dtos.Response;
using UsersMS.Core.Repositories;
using UsersMS.Infrastructure.Exceptions;

namespace UsersMS.Application.Handlers.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<GetAllUsersDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null) throw new UserNotFoundException("Users not found.");

            return users.Select(user => new GetAllUsersDto
            {
                UserId = user.Id!,
                Email = user.Email!,
                DocumentId = user.DocumentId!,
                Name = user.Name!,
                LastName = user.LastName!,
                Phone = user.Phone!,
                Address = user.Address!,
                Role = user.Role,
                State = user.State,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            }).ToList();
        }
    }
}

