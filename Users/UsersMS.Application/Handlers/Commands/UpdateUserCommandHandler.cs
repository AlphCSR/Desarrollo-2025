using MediatR;
using UsersMS.Application.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Infrastructure.Exceptions;
using UsersMS.Core.Service;
using UsersMS.Core.DataBase;
using FluentValidation;


namespace UsersMS.Application.Handlers.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IKeycloakService _keycloakService;
        private readonly IUsersDbContext _context;
        private readonly IValidator<UpdateUserCommand> _validator;

        public UpdateUserCommandHandler(IUserRepository userRepository, IKeycloakService keycloakService, IUsersDbContext context, IValidator<UpdateUserCommand> validator)
        {
            _userRepository = userRepository;
            _keycloakService = keycloakService;
            _context = context;
            _validator = validator;
        }

        public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidatorException(validationResult.Errors);

            var user = await _userRepository.GetByIdAsync(request.UpdateUserDto.UserId!);
            if (user == null)
                throw new UserNotFoundException("User not found.");

            if (!string.IsNullOrEmpty(request.UpdateUserDto.Email)) user.Email = request.UpdateUserDto.Email;
            if (!string.IsNullOrEmpty(request.UpdateUserDto.Password)) user.Password = request.UpdateUserDto.Password;
            if (!string.IsNullOrEmpty(request.UpdateUserDto.DocumentId)) user.DocumentId = request.UpdateUserDto.DocumentId;
            if (!string.IsNullOrEmpty(request.UpdateUserDto.Name)) user.Name = request.UpdateUserDto.Name;
            if (!string.IsNullOrEmpty(request.UpdateUserDto.LastName)) user.LastName = request.UpdateUserDto.LastName;
            if (!string.IsNullOrEmpty(request.UpdateUserDto.Phone)) user.Phone = request.UpdateUserDto.Phone;
            if (!string.IsNullOrEmpty(request.UpdateUserDto.Address)) user.Address = request.UpdateUserDto.Address;

            var token = await _keycloakService.GetAdminTokenAsync();

            var updatePayload = new
            {
                firstName = user.Name,
                lastName = user.LastName,
                email = user.Email,
                credentials = !string.IsNullOrEmpty(request.UpdateUserDto.Password)
                    ? new[] { new { type = "password", value = request.UpdateUserDto.Password, temporary = false } }
                    : null
            };

            await _keycloakService.UpdateUserAsync(user.Email!, updatePayload, token);
            await _userRepository.UpdateAsync(user);

            await _context.DbContext.SaveChangesAsync(cancellationToken);

            return "User updated successfully.";
        }
    }
}

