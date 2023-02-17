using AutoMapper;
using Bz.Fott.Registration.Domain.CompetitionIntegration;
using Bz.Fott.Registration.Domain.Competitors;

namespace Bz.Fott.Registration.Application.Competitors;

internal class CompetitorService : ICompetitorService
{
    private readonly ICompetitorRepository _competitorRepository;
    private readonly IMapper _mapper;

    public CompetitorService(
        ICompetitorRepository competitorRepository,
        IMapper mapper)
    {
        _competitorRepository = competitorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompetitorDto>> GetCompetitorsAsync(Guid competitionId)
    {
        var competitors = await _competitorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<Competitor>, IEnumerable<CompetitorDto>>(competitors);
    }
}
