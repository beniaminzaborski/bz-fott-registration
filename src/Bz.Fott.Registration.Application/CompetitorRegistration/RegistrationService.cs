using Bz.Fott.Registration.Domain.CompetitionIntegration;
using Bz.Fott.Registration.Domain.Competitors;
using FluentValidation;
using MassTransit;

namespace Bz.Fott.Registration.Application.CompetitorRegistration;

internal class RegistrationService : IRegistrationService
{
    private readonly IValidator<RegistrationRequestDto> _registrationRequestDtoValidator;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly ICompetitionRepository _competitionRepository;
    private readonly ICompetitorRepository _competitorRepository;

    public RegistrationService(
        IValidator<RegistrationRequestDto> registrationRequestDtoValidator,
        ISendEndpointProvider sendEndpointProvider,
        ICompetitionRepository competitionRepository,
        ICompetitorRepository competitorRepository)
    {
        _registrationRequestDtoValidator = registrationRequestDtoValidator;
        _sendEndpointProvider = sendEndpointProvider;
        _competitionRepository = competitionRepository;
        _competitorRepository = competitorRepository;
    }

    public async Task<Guid> RegisterAsync(RegistrationRequestDto dto)
    {
        await _registrationRequestDtoValidator.ValidateAndThrowAsync(dto);

        var competitionId = CompetitionId.From(dto.CompetitionId);

        var competition =  await _competitionRepository.GetAsync(competitionId)
            ?? throw new Common.Exceptions.NotFoundException("Competition does not exist");

        if (!competition.IsRegistrationOpen) throw new Common.Exceptions.ValidationException("Registration is closed");

        var numberOfRegistrations = await _competitorRepository.GetNumberOfRegisteredCompetitorsAsync(competitionId);
        if(numberOfRegistrations >= competition.MaxCompetitors) throw new Common.Exceptions.ValidationException("Registrations have reached the limit");

        var requestId = Guid.NewGuid();
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:register-competitor"));
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
