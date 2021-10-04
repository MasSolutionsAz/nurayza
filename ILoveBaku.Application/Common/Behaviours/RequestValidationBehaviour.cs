using FluentValidation;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.User.Commands.LoginUser;
using MediatR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Behaviours
{
    public class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var failures = _validators.Select(x => x.Validate(context))
                                       .SelectMany(x => x.Errors)
                                       .Where(x => x != null)
                                       .ToList();

            PropertyInfo errors = typeof(TRequest).GetProperty("Errors");

            MethodInfo errorsAdd = errors?.PropertyType.GetMethod("Add");

            object errorsValue = errors?.GetValue(request);

            foreach (var error in failures)
            {
                errorsAdd?.Invoke(errorsValue, new object[] { TakePropertyName(error.PropertyName), error.ErrorMessage });
            }

            //if (failures.Count > 0)
            //{
            //    return (TResponse)ApiResult<object>.CreateResponse(null, (Dictionary<string, string>)errorsValue,
            //        new ErrorDetail
            //        {
            //            ErrorMessage = "Validasiya erroru"
            //        });
            //}

            return await next();
        }

        private string TakePropertyName(string fullPropName)
        {
            return fullPropName.Split(".").TakeLast(1).FirstOrDefault();
        }
    }
}
