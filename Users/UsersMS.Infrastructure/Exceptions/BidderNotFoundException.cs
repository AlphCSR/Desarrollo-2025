using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Infrastructure.Exceptions
{
    public class BidderNotFoundException : Exception
    {
        public BidderNotFoundException() { }

        public BidderNotFoundException(string message)
            : base(message) { }

        public BidderNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
