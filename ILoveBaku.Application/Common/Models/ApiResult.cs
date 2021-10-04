using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ILoveBaku.Application.Common.Models
{
    public class ApiResult<TResponse> : Result<TResponse> where TResponse : notnull
    {
        private ApiResult() { }

        public static ApiResult<TResponse> CreateResponse(TResponse response = default, Dictionary<string,string> errorList = null, ErrorDetail error = null, bool? succeeded = null)
        {
            return new ApiResult<TResponse>()
            {
                Response = response,
                Succeeded = succeeded ?? (errorList.IsNull() && error.IsNull()),
                ErrorList = errorList,
                ErrorDetail = error
            };
        }

        public static ApiResult<TResponse> CultureResponse(ErrorDetail error = null)
        {
            return new ApiResult<TResponse>()
            {
                Succeeded = false,
                ErrorDetail = error ??= new ErrorDetail() { ErrorMessage = "Sorğu zamanı Header - da culture göndərilməlidi." }
            };
        }

        public static ApiResult<TResponse> ExceptionResponse(ErrorDetail error = null)
        {
            return new ApiResult<TResponse>()
            {
                Succeeded = false,
                ErrorDetail = error ??=  new ErrorDetail() { ErrorMessage = "Xəta baş verdi." }
            };
        }
    }
}
