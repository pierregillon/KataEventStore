using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace KataEventStore.TransactionDomain.Domain.Application._Base
{
    public abstract class CommandHandler<T> : IRequestHandler<T> where T : IRequest<Unit>
    {
        public async Task<Unit> Handle(T request, CancellationToken cancellationToken)
        {
            await Handle(request);
            return Unit.Value;
        }

        protected abstract Task Handle(T command);
    }

    public abstract class CommandHandler<T, TResult> : IRequestHandler<T, TResult> where T : IRequest<TResult>
    {
        public async Task<TResult> Handle(T request, CancellationToken cancellationToken) => await Handle(request);

        protected abstract Task<TResult> Handle(T command);
    }
}