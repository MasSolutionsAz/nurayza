using ILoveBaku.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Behaviours
{
    public class RequestLoggerBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        //private readonly IIdentityService _identityService;
        //private readonly ICurrentUserService _currentUserService;
        public RequestLoggerBehaviour(ILogger logger
                                     //  IIdentityService identityService,
                                     // ICurrentUserService currentUserService
                                     )
        {
            _logger = logger;
            //_identityService = identityService;
            //_currentUserService = _currentUserService;
        }
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            //var userId = _currentUserService.UserId; 
            //var userName = await _identityService.GetUserNameAsync(userId);

            _logger.LogInformation("Clean architecture Request: {Name} {@Request}",
                                            requestName, request);

        }
    }
}
