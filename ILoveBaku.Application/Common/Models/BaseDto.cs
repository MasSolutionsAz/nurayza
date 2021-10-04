using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Models
{
    public class BaseDto
    {
        public Dictionary<string, string> Errors { get; protected set; }

        public BaseDto()
        {
            Errors = new Dictionary<string, string>();
        }

        public void AddError(string propertyName, string errorMessage)
        {
            Errors.Add(propertyName, errorMessage);
        }

        public virtual void GetErrors(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
