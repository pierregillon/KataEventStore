namespace KataEventStore.TransactionDomain.Domain.Core._Base
{
    public abstract class AggregateRoot
    {
        protected void ApplyEvent(IDomainEvent domainEvent)
        {

        }
    }
}