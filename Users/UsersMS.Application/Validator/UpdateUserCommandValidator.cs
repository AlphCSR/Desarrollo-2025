using FluentValidation;
using UsersMS.Application.Commands;

namespace UsersMS.Application.Validator
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UpdateUserDto.UserId).NotEmpty().WithMessage("User ID cannot be empty.");
        }
    }
}