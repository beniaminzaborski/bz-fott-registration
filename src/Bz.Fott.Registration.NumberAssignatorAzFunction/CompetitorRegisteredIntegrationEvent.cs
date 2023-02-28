using System;

namespace Bz.Fott.Registration;

public sealed record CompetitorRegisteredIntegrationEvent(
        Guid CompetitionId,
        string FirstName,
        string LastName,
        DateTime BirthDate,
        string City,
        string PhoneNumber,
        string ContactPersonNumber,
        string Number)
{ }
