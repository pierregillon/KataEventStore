using System;
using System.Threading.Tasks;
using KataEventStore.TransactionDomain.Domain.Application._Base;
using KataEventStore.TransactionDomain.Domain.Core;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Application.CreateTransaction
{
    public class CreateTransactionCommandHandler : CommandHandler<CreateTransactionCommand, Guid>
    {
        private readonly IEventStore _eventStore;

        public CreateTransactionCommandHandler(IEventStore eventStore) => _eventStore = eventStore;

        protected override async Task<Guid> Handle(CreateTransactionCommand request)
        {
            var createdEvent = Transaction.New(request.Name, request.Amount);

            await _eventStore.Store(createdEvent);

            return createdEvent.AggregateId;
        }
    }
}