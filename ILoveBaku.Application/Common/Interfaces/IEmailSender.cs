using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string message);
      
    }
}
