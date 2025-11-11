
using Swashbuckle.AspNetCore.Filters;
using PaymentsMS.Application.Commands;

namespace PaymentsMS.Examples.Commands
{
    public class AttachPaymentMethodCommandExample : IExamplesProvider<AttachPaymentMethodCommand>
    {
        public AttachPaymentMethodCommand GetExamples()
        {
            return new AttachPaymentMethodCommand
            {
                CustomerId = "cus_xxxxxxxxxxxxxx",
                PaymentMethodId = "pm_card_visa"
            };
        }
    }
}
