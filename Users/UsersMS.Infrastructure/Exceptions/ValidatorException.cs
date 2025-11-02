using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace UsersMS.Infrastructure.Exceptions
{
    public class ValidatorException : Exception
    {
        public List<ValidationFailure> Errors { get; }
        public ValidatorException(List<ValidationFailure> errors)
            : base("La validación del objeto ha fallado.")
        {
            Errors = errors;
        }
    }
}
