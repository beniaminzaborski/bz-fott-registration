using Bz.Fott.Registration.Domain.CompetitionIntegration;
using Bz.Fott.Registration.Domain.Competitors;
using Bz.Fott.Registration.Infrastructure.Persistence.Common;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Repositories;

internal class CompetitorRepository : Repository<Competitor, CompetitorId, ApplicationDbContext>, ICompetitorRepository
{
    public CompetitorRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
