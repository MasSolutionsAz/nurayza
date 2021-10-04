using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Behaviours
{
    public class RequestExceptionHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException> where TException : Exception
    {
        private readonly IApplicationDbContext _context;

        private readonly HttpContext _httpContext;

        public RequestExceptionHandler(IApplicationDbContext context,
                                        IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext.HttpContext;
        }

        public async Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
        {
            string url = _httpContext?.Request?.Path.Value ?? string.Empty;

            try
            {
                if (!_context.Database.CurrentTransaction.IsNull())
                {
                    await _context.Database.CurrentTransaction?.RollbackAsync(cancellationToken);
                       
                    await _context.ErrorLogs.AddAsync(new ErrorLogs
                    {
                        LogText = GetExceptionLog(exception),
                        Url = url,
                        CreatedDate = DateTime.Now,
                        CreatedIp = DateTime.Now
                    });

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception exp)
            {
                await _context.ErrorLogs.AddAsync(new ErrorLogs
                {
                    LogText = $"Exception throw in RequestExceptionHandler \nMessage: {exp.Message}",
                    Url = url,
                    CreatedDate = DateTime.Now,
                    CreatedIp = DateTime.Now
                });

                await _context.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                state.SetHandled(GetExceptionResponse());
            }
        }

        private string GetExceptionLog(TException exception)
        {
            StringBuilder result = new StringBuilder();
            foreach (StackFrame frame in new StackTrace(exception, true).GetFrames())
            {
                MethodBase method = frame.GetMethod();
                result.Append($"\nLocation: {method?.ReflectedType?.Name}.{method?.Name}(), Line: {frame.GetFileLineNumber()}, Column: {frame.GetFileColumnNumber()}");
                if (method.IsFinal) // typeof(StackFrame).GetProperty("IsLastFrameFromForeignExceptionStackTrace", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(frame)
                    break;
            }
            return $"Request: {typeof(TRequest).Name}. \nMessage: {exception.Message} {result}";
        }

        private TResponse GetExceptionResponse(string errorMessage = "Xəta baş verdi.")
        {
            return (TResponse)typeof(TResponse)
                                 .GetMethod("ExceptionResponse")?
                                    .Invoke((TResponse)Activator.CreateInstance(typeof(TResponse), true),
                                            new object[] { new ErrorDetail() { ErrorMessage = errorMessage } });
        }
    }
}
