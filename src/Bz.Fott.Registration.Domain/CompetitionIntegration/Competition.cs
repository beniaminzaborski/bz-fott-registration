using Bz.Fott.Registration.Domain.Common;

namespace Bz.Fott.Registration.Domain.CompetitionIntegration;

public class Competition : Entity<CompetitionId>, IAggregateRoot
{
    private Competition() { }

    public Competition(
        CompetitionId id,
        int maxCompetitors,
        bool isRegistrationOpen)
    {
        Id = id;
        MaxCompetitors = maxCompetitors;
        IsRegistrationOpen = isRegistrationOpen;
    }

    public int MaxCompetitors { get; private set; }

    public bool IsRegistrationOpen { get; private set; }

    public void CloseRegistration()
    { 
        IsRegistrationOpen = false;
    }

    public void IncreaseMaxCompetitorsTo(int maxCompetitors)
    {
        MaxCompetitors = maxCompetitors;
    }
}
