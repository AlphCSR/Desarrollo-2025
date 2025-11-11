using MediatR;

namespace PaymentsMS.Application.Commands
{
    public class SetDefaultPaymentMethodCommand : IRequest<bool>
    {
        public string CustomerId { get; set; } = string.Empty;
        public string PaymentMethodId { get; set; } = string.Empty;
    }
}
