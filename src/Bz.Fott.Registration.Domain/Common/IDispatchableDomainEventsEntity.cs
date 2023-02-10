namespace Bz.Fott.Registration.Domain.Common;

public interface IDispatchableDomainEventsEntity
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();

    public void ClearDomainEvents();
}
