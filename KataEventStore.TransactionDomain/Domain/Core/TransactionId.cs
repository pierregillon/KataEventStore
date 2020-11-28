using System;

namespace KataEventStore.TransactionDomain.Domain.Core
{
    public readonly struct TransactionId : IComparable<TransactionId>
    {
        private readonly Guid _guid;
        
        public static TransactionId New() => new TransactionId(Guid.NewGuid());
        
        private TransactionId(Guid guid) => this._guid = guid;

        public bool Equals(TransactionId other) => _guid.Equals(other._guid);

        public override bool Equals(object obj) => obj is TransactionId other && Equals(other);

        public override int GetHashCode() => _guid.GetHashCode();

        public int CompareTo(TransactionId other) => _guid.CompareTo(other._guid);

        public static implicit operator Guid(TransactionId transactionId) => transactionId._guid;

        public static bool operator ==(TransactionId transactionId, TransactionId transactionId2) => transactionId.Equals(transactionId2);
        public static bool operator !=(TransactionId transactionId, TransactionId transactionId2) => !(transactionId == transactionId2);
    }
}