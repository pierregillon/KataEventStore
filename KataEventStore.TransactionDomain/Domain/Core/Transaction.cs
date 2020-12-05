using System;
using System.Collections.Generic;
using System.Linq;
using KataEventStore.Events;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Core
{
    public class Transaction : AggregateRoot
    {
        private TransactionId _id;
        private decimal _amount;
        private string _name;

        public static TransactionCreated New(string name, in decimal amount) 
            => new TransactionCreated(TransactionId.New(), name, amount);

        public static Transaction Rehydrate(IEnumerable<IDomainEvent> events) 
            => new Transaction(events.ToArray());

        protected Transaction(IReadOnlyCollection<IDomainEvent> events)
        {
            if (!events.Any()) {
                throw new InvalidOperationException("The transaction does not have any events to apply.");
            }
            if (events.OfType<TransactionDeleted>().Any()) {
                throw new InvalidOperationException("Unable to load the transaction : it has been deleted.");
            }
            foreach (var domainEvent in events) {
                ApplyEvent(domainEvent);
            }
        }

        public IEnumerable<IDomainEvent> Rename(string newName)
        {
            if (_name != newName) {
                yield return new TransactionRenamed(_id, _name, newName);
            }
        }

        public IEnumerable<IDomainEvent> EditAmount(decimal newAmount)
        {
            if (_amount != newAmount) {
                yield return new TransactionAmountEdited(_id, _amount, newAmount);
            }
        }

        public IEnumerable<IDomainEvent> Delete()
        {
            yield return new TransactionDeleted(_id);
        }

        protected void Apply(TransactionCreated @event)
        {
            _id = TransactionId.From(@event.AggregateId);
            _amount = @event.Amount;
            _name = @event.Name;
        }

        protected void Apply(TransactionRenamed @event)
        {
            _name = @event.NewName;
        }

        protected void Apply(TransactionAmountEdited @event)
        {
            _amount = @event.NewAmount;
        }
    }
}