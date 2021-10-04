using ILoveBaku.Application.Common.Interfaces;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Behaviours
{
    public class RequestBeginTransactionBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IApplicationDbContext _context;

        public RequestBeginTransactionBehaviour(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            //make log
            await _context.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
