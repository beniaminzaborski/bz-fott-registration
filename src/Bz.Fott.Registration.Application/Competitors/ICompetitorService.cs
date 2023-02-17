using Bz.Fott.Registration.Application.Common;

namespace Bz.Fott.Registration.Application.Competitors;

public interface ICompetitorService : IApplicationService
{
    Task<IEnumerable<CompetitorDto>> GetCompetitorsAsync(Guid competitionId);
}
