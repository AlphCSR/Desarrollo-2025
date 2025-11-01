using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Infrastructure.Exceptions
{
    public class ValidatorException : Exception
    {
        //para poder pasarle una coleccion de objetos que devuelve validateAsync, se tuvo que crear este constructor que recibe
        //una lista de ese tipo de objetos 
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
