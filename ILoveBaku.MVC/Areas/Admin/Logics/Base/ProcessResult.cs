using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Base
{
    public class ProcessResult<TResponse>
    {
        public Dictionary<string, string> Errors { get; private set; }
        public bool Succeeded { get; private set; }
        public bool IsUpdate { get; private set; }
        public TResponse Response { get; private set; }

        private ProcessResult() { }

        public static ProcessResult<TResponse> CreateResult(TResponse Response,bool IsUpdate=false, Dictionary<string, string> errors = null, bool Succeeded=true) 
        {
            return new ProcessResult<TResponse>
            {
                Response = Response,
                IsUpdate = IsUpdate,
                Errors = errors,
                Succeeded = Response != null
            };
        }
    }
}
