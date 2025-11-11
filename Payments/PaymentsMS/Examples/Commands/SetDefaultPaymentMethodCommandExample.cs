
using Swashbuckle.AspNetCore.Filters;
using PaymentsMS.Application.Commands;

namespace PaymentsMS.Examples.Commands
{
    public class SetDefaultPaymentMethodCommandExample : IExamplesProvider<SetDefaultPaymentMethodCommand>
    {
        public SetDefaultPaymentMethodCommand GetExamples()
        {
            return new SetDefaultPaymentMethodCommand
            {
                CustomerId = "cus_xxxxxxxxxxxxxx",
                PaymentMethodId = "pm_xxxxxxxxxxxxxx"
            };
        }
    }
}
