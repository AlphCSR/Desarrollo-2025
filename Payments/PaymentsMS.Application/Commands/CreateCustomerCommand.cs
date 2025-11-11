using MediatR;

namespace PaymentsMS.Application.Commands
{
    public class CreateCustomerCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
