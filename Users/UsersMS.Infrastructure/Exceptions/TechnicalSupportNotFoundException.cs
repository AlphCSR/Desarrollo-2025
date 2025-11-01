using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Infrastructure.Exceptions
{
    public class TechnicalSupportNotFoundException : Exception
    {
        public TechnicalSupportNotFoundException() { }

        public TechnicalSupportNotFoundException(string message)
            : base(message) { }

        public TechnicalSupportNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
