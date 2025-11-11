using MediatR;

namespace PaymentsMS.Application.Commands
{
    public class ProcessPaymentCommand : IRequest<bool>
    {
        public string CustomerId { get; set; } = string.Empty;
        public string PaymentMethodId { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}
