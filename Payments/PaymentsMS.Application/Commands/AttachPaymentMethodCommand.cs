using MediatR;

namespace PaymentsMS.Application.Commands
{
    public class AttachPaymentMethodCommand : IRequest<string>
    {
        public string CustomerId { get; set; } = string.Empty;
        public string PaymentMethodId { get; set; } = string.Empty;
    }
}
