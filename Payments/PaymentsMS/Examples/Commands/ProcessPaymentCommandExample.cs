
using Swashbuckle.AspNetCore.Filters;
using PaymentsMS.Application.Commands;

namespace PaymentsMS.Examples.Commands
{
    public class ProcessPaymentCommandExample : IExamplesProvider<ProcessPaymentCommand>
    {
        public ProcessPaymentCommand GetExamples()
        {
            return new ProcessPaymentCommand
            {
                CustomerId = "cus_xxxxxxxxxxxxxx",
                Amount = 1000,
                Currency = "usd",
                PaymentMethodId = "pm_xxxxxxxxxxxxxx"
            };
        }
    }
}
