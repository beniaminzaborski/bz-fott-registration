using Bz.Fott.Registration.Application.Common;
using Bz.Fott.Registration.Domain.Competitors;
using Bz.Fott.Telemetry.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Bz.Fott.Registration.Application.CompetitorIntegration;

public class CompetitorConsumer : IConsumer<CompetitorTimeCalculatedIntegrationEvent>
{
    private readonly ILogger<CompetitorConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompetitorRepository _competitorRepository;

    public CompetitorConsumer(
        ILogger<CompetitorConsumer> logger,
        IUnitOfWork unitOfWork,
        ICompetitorRepository competitorRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _competitorRepository = competitorRepository;
    }

    public async Task Consume(ConsumeContext<CompetitorTimeCalculatedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation($"Time calculated for competitor id: {message.CompetitorId}");

        var competitor = await _competitorRepository.GetAsync(CompetitorId.From(message.CompetitorId));
        if (competitor is not null)
        {
            competitor.SetNetTime(message.NetTime);
            await _unitOfWork.CommitAsync();
        }
    }
}

public class CompetitorConsumerDefinition : ConsumerDefinition<CompetitorConsumer>
{
    public CompetitorConsumerDefinition()
    {
        EndpointName = "competitor-time-calculated-events-to-registr-service";
    }
}
