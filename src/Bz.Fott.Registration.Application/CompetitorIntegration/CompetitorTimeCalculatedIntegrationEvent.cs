using System;

namespace Bz.Fott.Telemetry.IntegrationEvents;

public sealed record CompetitorTimeCalculatedIntegrationEvent(
    Guid CompetitorId,
    TimeSpan NetTime)
{ }
