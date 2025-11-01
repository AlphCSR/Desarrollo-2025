using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Core.Service
{
    public interface IEmailService
    {
        Task SendEmail(string receptor);
        Task SendPassword(string receptor, int code);
    }
}
