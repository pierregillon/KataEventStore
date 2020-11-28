using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace KataEventStore.TransactionDomain.Domain.Application._Base
{
    public abstract class CommandHandler<T> : IRequestHandler<T> where T : IRequest<Unit>
    {
        public async Task<Unit> Handle(T request, CancellationToken cancellationToken)
        {
            await this.Handle(request);
            return Unit.Value;
        }

        protected abstract Task Handle(T command);
    }
}