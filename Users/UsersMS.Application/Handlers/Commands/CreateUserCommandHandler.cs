using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UsersMS.Application.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Core.Service;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.Service;

namespace UsersMS.Application.Handlers.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IKeycloakService _keycloakService;
        private readonly IEventPublisher _eventPublisher;

        public CreateUserCommandHandler(IUserRepository userRepository, IKeycloakService keycloakService,IEventPublisher eventPublisher)
        {
            _userRepository = userRepository;
            _keycloakService = keycloakService;
            _eventPublisher = eventPublisher;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CreateUserDto;

            var user = new User(
                dto.Email!,
                dto.Password!,
                dto.DocumentId!,
                dto.Name!,
                dto.LastName!,
                dto.Phone!,
                dto.Address!,
                dto.Role,
                dto.State
            );

            await _userRepository.AddAsync(user);

            var token = await _keycloakService.GetAdminTokenAsync();

            var keycloakUser = new
            {
                username = user.Email,
                firstName = user.Name,
                lastName = user.LastName,
                email = user.Email,
                enabled = true,
                credentials = new[]
                {
                    new { type = "password", value = user.Password, temporary = false }
                },
            };

            await _keycloakService.CreateUserAsync(keycloakUser, token);
            await _keycloakService.AssignRoleAsync(user.Email!, user.Role.ToString(), token);
            await _eventPublisher.PublishUserCreatedAsync(user);


            return "User successfully created.";
        }
    }
}
