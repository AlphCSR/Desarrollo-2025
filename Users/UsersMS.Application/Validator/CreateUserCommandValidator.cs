using FluentValidation;
using UsersMS.Application.Commands;
using UsersMS.Commons.Dtos.Request;

namespace UsersMS.Application.Validator
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IValidator<CreateUserDto> createUserValidator)
        {
            RuleFor(x => x.CreateUserDto).SetValidator(createUserValidator);
        }
    }
}
