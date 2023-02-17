using Bz.Fott.Registration.Application.Competitors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bz.Fott.Registration.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompetitorController : ControllerBase
{
    private readonly ICompetitorService _competitorService;

    public CompetitorController(ICompetitorService competitorService)
    {
        _competitorService = competitorService; 
    }

    [HttpGet("{competitionId:Guid}")]
    [ProducesResponseType(typeof(IEnumerable<CompetitorDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync(Guid competitionId)
    {
        var result = await _competitorService.GetCompetitorsAsync(competitionId);
        return Ok(result);
    }
}
