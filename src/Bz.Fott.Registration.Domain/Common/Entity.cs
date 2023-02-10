namespace Bz.Fott.Registration.Domain.Common;

public abstract class Entity<TId> : IDispatchableDomainEventsEntity
{
    public TId Id { get; protected set; }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Entity<TId> entity)
        {
            return entity.Id.Equals(Id);
        }

        return false;
    }

    private IList<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public void QueueDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
