using System;

namespace KataEventStore.TransactionDomain.Domain.Core
{
    public readonly struct TransactionId : IComparable<TransactionId>
    {
        private readonly Guid guid;

        public TransactionId(Guid guid)
        {
            this.guid = guid;
        }

        public bool Equals(TransactionId other)
        {
            return guid.Equals(other.guid);
        }

        public override bool Equals(object obj)
        {
            return obj is TransactionId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return guid.GetHashCode();
        }

        public int CompareTo(TransactionId other)
        {
            return guid.CompareTo(other.guid);
        }

        public static implicit operator Guid(TransactionId transactionId) => transactionId.guid;

        public static TransactionId New()
        {
            return new TransactionId(Guid.NewGuid());
        }
    }
}