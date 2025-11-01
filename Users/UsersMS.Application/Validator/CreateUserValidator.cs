using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Commons.Dtos.Request;

namespace UsersMS.Application.Validator
{
    public class CreateUserValidator : ValidatorBase<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(s => s.Email)
                .NotNull().WithMessage("Email no puede ser nulo").WithErrorCode("010")
                .NotEmpty().WithMessage("Email no puede estar vacío").WithErrorCode("012")
                .EmailAddress().WithMessage("Email debe ser un correo electrónico válido").WithErrorCode("011")
                .MaximumLength(100).WithMessage("Email no puede tener más de 100 caracteres").WithErrorCode("013");

            RuleFor(s => s.Password)
                .NotNull().WithMessage("Password no puede ser nulo").WithErrorCode("020")
                .MinimumLength(6).WithMessage("Password debe tener al menos 6 caracteres").WithErrorCode("021")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")
                .WithMessage("Password debe tener al menos 8 caracteres, incluyendo una letra y un número")
                .WithErrorCode("022");

            RuleFor(s => s.Name).NotNull().WithMessage("Name no puede ser nulo").WithErrorCode("040");
            RuleFor(s => s.LastName).NotNull().WithMessage("Apellido no puede ser nulo").WithErrorCode("050");
            RuleFor(s => s.DocumentId).NotNull().WithMessage("Cédula no puede ser nula").WithErrorCode("030");
            RuleFor(s => s.Role).IsInEnum().WithMessage("Rol inválido").WithErrorCode("060");
            RuleFor(s => s.Phone)
                .NotNull().WithMessage("Teléfono no puede ser nulo").WithErrorCode("080")
                .NotEmpty().WithMessage("Teléfono no puede estar vacío").WithErrorCode("081");

            RuleFor(s => s.Address).NotNull().WithMessage("Dirección no puede ser nula").WithErrorCode("090");
            RuleFor(s => s.State).IsInEnum().WithMessage("Estado inválido").WithErrorCode("070");
        }
    }
}

