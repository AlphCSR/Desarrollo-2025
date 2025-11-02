using FluentValidation;
using UsersMS.Commons.Dtos.Request;

namespace UsersMS.Application.Validator
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name cannot be empty.");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone number cannot be empty.");
        }
    }
}