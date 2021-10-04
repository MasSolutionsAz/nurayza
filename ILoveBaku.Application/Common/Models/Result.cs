using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.Common.Models
{
    public abstract class Result<TResponse>
    {
        public virtual TResponse Response { get;  set; }
        public virtual bool Succeeded { get;  set; }
        public virtual ErrorDetail  ErrorDetail { get;  set; }
        public virtual Dictionary<string,string>  ErrorList { get;  set; }
    }
}
