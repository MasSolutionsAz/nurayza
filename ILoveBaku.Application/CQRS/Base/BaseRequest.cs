using ILoveBaku.Application.Common.Extension;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Base
{
    public class BaseRequest<TResponse> : IRequest<TResponse>
    {
        Dictionary<string, string> _errors;

        public Dictionary<string, string> Errors
        {
            get
            {
                return _errors ??= new Dictionary<string, string>();
            }
            set
            {
                if (!value.IsNull())
                    _errors = value;
            }
        }

        protected string Culture { get; set; }

        protected Guid UserId { get; set; }
    }
}
