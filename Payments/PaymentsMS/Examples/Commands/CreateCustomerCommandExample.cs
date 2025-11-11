
using Swashbuckle.AspNetCore.Filters;
using PaymentsMS.Application.Commands;

namespace PaymentsMS.Examples.Commands
{
    public class CreateCustomerCommandExample : IExamplesProvider<CreateCustomerCommand>
    {
        public CreateCustomerCommand GetExamples()
        {
            return new CreateCustomerCommand
            {
                Email = "test@example.com",
                Name = "Test Customer"
            };
        }
    }
}
