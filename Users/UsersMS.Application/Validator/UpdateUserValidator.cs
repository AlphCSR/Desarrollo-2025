using FluentValidation;
using UsersMS.Commons.Dtos.Request;

namespace UsersMS.Application.Validator
{
    public class UpdateUserValidator : ValidatorBase<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(s => s.UserId).NotNull().WithMessage("UserId no puede ser nulo").WithErrorCode("001");

            When(s => s.Email != null, () =>
            {
                RuleFor(s => s.Email)
                    .EmailAddress().WithMessage("Email debe ser un correo electrónico válido").WithErrorCode("011")
                    .MaximumLength(100).WithMessage("Email no puede tener más de 100 caracteres").WithErrorCode("013");
            });

            When(s => s.Password != null, () =>
            {
                RuleFor(s => s.Password)
                    .MinimumLength(6).WithMessage("Password debe tener al menos 6 caracteres").WithErrorCode("021")
                    .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")
                    .WithMessage("Password debe tener al menos 8 caracteres, incluyendo una letra y un número")
                    .WithErrorCode("022");
            });

            When(s => s.Phone != null, () =>
            {
                RuleFor(s => s.Phone)
                    .NotEmpty().WithMessage("Teléfono no puede estar vacío").WithErrorCode("081");
            });
        }
    }
}
