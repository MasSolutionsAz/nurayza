using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Behaviours
{
    public class RequestCommitTransactionBehaviour<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> 
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        public RequestCommitTransactionBehaviour(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            //make log
            if (!_context.Database.CurrentTransaction.IsNull())
                await _context.Database.CurrentTransaction?.CommitAsync(cancellationToken);
        }
    }
}
