using System;

namespace Bz.Fott.Registration.Application.CompetitorRegistration;

public record RegisterCompetitor
{
    public Guid RequestId { get; init; }
    public Guid CompetitionId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime BirthDate { get; init; }
    public string City { get; init; }
    public string PhoneNumber { get; init; }
    public string ContactPersonNumber { get; init; }
}
