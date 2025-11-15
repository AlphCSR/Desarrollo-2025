using FluentValidation;
using EventsMS.Commons.Dtos.Request;

namespace EventsMS.Application.Validator
{
    public class CreateEventValidator : AbstractValidator<CreateEventDto>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("La capacidad debe ser mayor a 0.");
            RuleFor(x => x.StartDate).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de inicio no puede ser en el pasado.");
            RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");
        }
    }
}