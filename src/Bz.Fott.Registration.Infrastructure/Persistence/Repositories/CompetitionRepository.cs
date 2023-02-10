using Bz.Fott.Registration.Domain.CompetitorRegistration;
using Bz.Fott.Registration.Infrastructure.Persistence.Common;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Repositories;

internal class CompetitionRepository : Repository<Competition, CompetitionId, ApplicationDbContext>, ICompetitionRepository
{
    public CompetitionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
