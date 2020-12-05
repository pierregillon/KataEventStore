using System;
using System.Collections.Generic;
using KataEventStore.Events;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Core
{
    public class Transaction : AggregateRoot
    {
        private TransactionId _id;
        private bool _hasBeenDeleted;
        private decimal _amount;
        private string _name;

        public static IDomainEvent New(string name, in decimal amount) 
            => new TransactionCreated(TransactionId.New(), name, amount);

        public static Transaction Rehydrate(IEnumerable<IDomainEvent> events) 
            => new Transaction(events);

        protected Transaction(IEnumerable<IDomainEvent> events)
        {
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
            if (_hasBeenDeleted) {
                throw new InvalidOperationException("Unable to deleted transaction : already deleted.");
            }
            yield return new TransactionDeleted(_id);
        }

        protected void Apply(TransactionCreated @event)
        {
            _id = TransactionId.From(@event.AggregateId);
            _amount = @event.Amount;
            _name = @event.Name;
        }

        protected void Apply(TransactionDeleted @event)
        {
            _hasBeenDeleted = true;
        }
    }
}