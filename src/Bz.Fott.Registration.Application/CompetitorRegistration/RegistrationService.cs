using Bz.Fott.Registration.Domain.CompetitionIntegration;
using FluentValidation;
using MassTransit;

namespace Bz.Fott.Registration.Application.CompetitorRegistration;

internal class RegistrationService : IRegistrationService
{
    private readonly IValidator<RegistrationRequestDto> _registrationRequestDtoValidator;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly ICompetitionRepository _competitionRepository;

    public RegistrationService(
        IValidator<RegistrationRequestDto> registrationRequestDtoValidator,
        ISendEndpointProvider sendEndpointProvider,
        ICompetitionRepository competitionRepository)
    {
        _registrationRequestDtoValidator = registrationRequestDtoValidator;
        _sendEndpointProvider = sendEndpointProvider;
        _competitionRepository = competitionRepository;
    }

    public async Task<Guid> RegisterAsync(RegistrationRequestDto dto)
    {
        await _registrationRequestDtoValidator.ValidateAndThrowAsync(dto);

        var competition =  await _competitionRepository.GetAsync(CompetitionId.From(dto.CompetitionId));
        if (competition is null) throw new Common.Exceptions.NotFoundException("Competition does not exist");
        if(!competition.IsRegistrationOpen) throw new Common.Exceptions.ValidationException("Registration is closed");

        var requestId = Guid.NewGuid();
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:registrations"));
        await endpoint.Send(new RegisterCompetitor(
            requestId,
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
