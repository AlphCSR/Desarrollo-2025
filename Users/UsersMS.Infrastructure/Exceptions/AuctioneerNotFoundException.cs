using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Infrastructure.Exceptions
{
    public class AuctioneerNotFoundException : Exception
    {
        public AuctioneerNotFoundException() { }

        public AuctioneerNotFoundException(string message)
            : base(message) { }

        public AuctioneerNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
