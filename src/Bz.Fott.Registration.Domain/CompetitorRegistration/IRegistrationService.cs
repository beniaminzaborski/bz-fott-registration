using Bz.Fott.Registration.Domain.Common;

namespace Bz.Fott.Registration.Domain.CompetitorRegistration;

public interface IRegistrationService : IDomainService
{
    Task RegisterAsync();
}
