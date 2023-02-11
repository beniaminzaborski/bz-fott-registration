namespace Bz.Fott.Registration.Application.CompetitorRegistration;

public record RegisterCompetitor(
    Guid CompetitionId,
    string FirstName,
    string LastName,
    DateTime BirthDate,
    string City,
    string PhoneNumber,
    string ContactPersonNumber)
{
}
