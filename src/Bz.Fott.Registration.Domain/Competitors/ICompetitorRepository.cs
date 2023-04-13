using Bz.Fott.Registration.Domain.Common;
using Bz.Fott.Registration.Domain.CompetitionIntegration;

namespace Bz.Fott.Registration.Domain.Competitors;

public interface ICompetitorRepository : IRepository<Competitor, CompetitorId>
{
    Task<IEnumerable<Competitor>> GetAllByCompetitionIdAsync(CompetitionId competitionId);

    Task<int> GetNumberOfRegisteredCompetitorsAsync(CompetitionId competitionId);
}
