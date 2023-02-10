namespace Bz.Fott.Administration.Application.Competitions;

public sealed record CompetitionOpenedForRegistrationIntegrationEvent(
    Guid Id,
    int MaxCompetitors)
{ }
