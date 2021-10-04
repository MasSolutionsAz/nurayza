using ILoveBaku.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.Common.Models
{
    public class ErrorDetail
    {
        public StatusCode StatusCode { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
