using Bz.Fott.Registration.Domain.Common;
using Bz.Fott.Registration.Domain.CompetitionIntegration;
using Bz.Fott.Registration.Domain.Competitors;
using Bz.Fott.Registration.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Repositories;

internal class CompetitorRepository : Repository<Competitor, CompetitorId, ApplicationDbContext>, ICompetitorRepository
{
    public CompetitorRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Competitor>> GetAllByCompetitionIdAsync(CompetitionId competitionId)
    {
        return await _dbContext.Set<Competitor>()
            .Where(c => c.CompetitionId.Equals(competitionId))
            .ToListAsync();
    }
}
