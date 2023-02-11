using Bz.Fott.Registration.Application.Common;

namespace Bz.Fott.Registration.Application.CompetitorRegistration;

public interface IRegistrationService : IApplicationService
{
    Task<Guid> RegisterAsync(RegistrationRequestDto dto);
}
