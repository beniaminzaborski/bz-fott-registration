using MassTransit;
using System;

namespace Bz.Fott.Registration;

[EntityName("registration-completed")]
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
