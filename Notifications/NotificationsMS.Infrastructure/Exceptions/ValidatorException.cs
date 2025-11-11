using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsMS.Infrastructure.Exceptions
{
    public class ValidatorException : Exception
    {

        public ValidatorException(List<FluentValidation.Results.ValidationFailure> errors)
        {
        }

        public ValidatorException(string message)
            : base(message)
        {
        }

        public ValidatorException(string message, Exception inner)
        : base(message, inner)
        {
        }

        //public ValidatorException(IEnumerable<ValidationFailure> errors) : base("Validation failed") { }

    }
}
