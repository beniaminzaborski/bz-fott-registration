using Bz.Fott.Registration.Application.CompetitorRegistration;
using MassTransit;
using System.Threading.Tasks;

namespace Bz.Fott.Registration.NumberAssignatorAzFunction.Consumers;

public class RegisterCompetitorConsumer : IConsumer<RegisterCompetitor>
{
    private readonly ICompetitorService _competitorService;

    public RegisterCompetitorConsumer(ICompetitorService competitorService)
    {
        _competitorService = competitorService;
    }

    public async Task Consume(ConsumeContext<RegisterCompetitor> context)
    {
        var registerCompetitor = context.Message;

        LogContext.Debug?.Log("Request competitor registration {FirstName} {LastName} on competition {CompetitionId}",
           registerCompetitor.FirstName,
           registerCompetitor.LastName,
           registerCompetitor.CompetitionId);

        string number = await _competitorService.RegisterCompetitorAndReturnNumber(registerCompetitor);

        if (!string.IsNullOrEmpty(number))
        {
            await context.Publish(new CompetitorRegisteredIntegrationEvent(
                registerCompetitor.CompetitionId,
                registerCompetitor.FirstName,
                registerCompetitor.LastName,
                registerCompetitor.BirthDate,
                registerCompetitor.City,
                registerCompetitor.PhoneNumber,
                registerCompetitor.ContactPersonNumber,
                number.ToString()
            ));
        }
    }
}
