using System;
using System.Collections.Generic;
using KataEventStore.TransactionDomain.Domain.Core._Base;
using KataEventStore.TransactionDomain.Domain.Core.Events;

namespace KataEventStore.TransactionDomain.Domain.Core
{
    public class Transaction : AggregateRoot
    {
        private TransactionId _id;
        private bool _hasBeenDeleted;
        private decimal _amount;
        private string _name;

        public static IDomainEvent New(string name, in decimal amount)
        {
            return new TransactionCreated(TransactionId.New(), name, amount);
        }

        public static Transaction Rehydrate(IEnumerable<IDomainEvent> events)
        {
            return new Transaction(events);
        }

        protected Transaction(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events) {
                this.ApplyEvent(domainEvent);
            }
        }

        public IDomainEvent EditAmount(decimal newAmount)
        {
            if (this._amount != newAmount) {
                return new TransactionAmountEdited(this._id, this._amount, newAmount);
            }
            return null;
        }

        public IDomainEvent Delete()
        {
            if (_hasBeenDeleted) {
                throw new InvalidOperationException("Unable to deleted transaction : already deleted.");
            }
            return new TransactionDeleted(this._id);
        }

        protected void Apply(TransactionCreated @event)
        {
            this._id = @event.Id;
            this._amount = @event.Amount;
            this._name = @event.Name;
        }

        protected void Apply(TransactionDeleted @event)
        {
            this._hasBeenDeleted = true;
        }

        public IDomainEvent Rename(string newName)
        {
            if (this._name != newName) {
                return new TransactionRenamed(this._id, this._name, newName);
            }
            return null;
        }
    }
}