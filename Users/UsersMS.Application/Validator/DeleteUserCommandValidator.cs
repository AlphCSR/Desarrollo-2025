using FluentValidation;
using UsersMS.Application.Commands;

namespace UsersMS.Application.Validator
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.DeleteUserDto.UserId).NotEmpty().WithMessage("User ID cannot be empty.");
        }
    }
}