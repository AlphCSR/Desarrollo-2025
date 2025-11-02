using MediatR;
using FluentValidation;
using UsersMS.Application.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Core.Service;
using UsersMS.Core.DataBase;
using UsersMS.Infrastructure.Exceptions;


namespace UsersMS.Application.Handlers.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IKeycloakService _keycloakService;
        private readonly IUsersDbContext _context;
        private readonly IValidator<DeleteUserCommand> _validator;


        public DeleteUserCommandHandler(IUserRepository userRepository, IKeycloakService keycloakService, IUsersDbContext context, IValidator<DeleteUserCommand> validator  )
        {
            _userRepository = userRepository;
            _keycloakService = keycloakService;
            _context = context;
            _validator = validator;
        }

        public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidatorException(validationResult.Errors);

            var userId = request.DeleteUserDto.UserId;

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ApplicationException($"No user found with ID {userId}");

            if (string.IsNullOrEmpty(user.Email))
                throw new ApplicationException("User email cannot be null or empty.");

            var token = await _keycloakService.GetAdminTokenAsync();
            await _keycloakService.DisableUserAsync(user.Email, token);
            await _userRepository.DeleteAsync(userId);

            await _context.DbContext.SaveChangesAsync(cancellationToken);

            return "User successfully disabled.";
        }
    }
}

