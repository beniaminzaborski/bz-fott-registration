using FluentValidation;
using MassTransit;

namespace Bz.Fott.Registration.Application.CompetitorRegistration;

internal class RegistrationService : IRegistrationService
{
    private readonly IValidator<RegistrationRequestDto> _registrationRequestDtoValidator;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public RegistrationService(
        IValidator<RegistrationRequestDto> registrationRequestDtoValidator,
        ISendEndpointProvider sendEndpointProvider)
    {
        _registrationRequestDtoValidator = registrationRequestDtoValidator;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task<Guid> RegisterAsync(RegistrationRequestDto dto)
    {
        await _registrationRequestDtoValidator.ValidateAndThrowAsync(dto);

        var requestId = Guid.NewGuid();

        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:registrations"));
        await endpoint.Send(new RegisterCompetitor(
            dto.CompetitionId,
            dto.FirstName,
            dto.LastName,
            dto.BirthDate,
            dto.City,
            dto.PhoneNumber,
            dto.ContactPersonNumber));

        return requestId;
    }
}
