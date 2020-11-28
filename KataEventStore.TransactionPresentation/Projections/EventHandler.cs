using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace KataEventStore.TransactionPresentation.Projections {
    public abstract class EventHandler<T> : IRequestHandler<T> where T : IRequest
    {
        public async Task<Unit> Handle(T request, CancellationToken cancellationToken)
        {
            await this.Handle(request);
            return Unit.Value;
        }

        protected abstract Task Handle(T @event);
    }
}