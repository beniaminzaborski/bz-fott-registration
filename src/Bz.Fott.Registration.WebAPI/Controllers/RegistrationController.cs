using Bz.Fott.Registration.Application.CompetitorRegistration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bz.Fott.Registration.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegistrationRequestDto dto)
    {
        
        return Accepted(new { RequestId = Guid.NewGuid() });
    }

}
