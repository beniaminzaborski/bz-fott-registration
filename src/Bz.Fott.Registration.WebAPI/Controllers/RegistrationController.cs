using Bz.Fott.Registration.Application.CompetitorRegistration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bz.Fott.Registration.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegistrationRequestDto dto)
    {
        var requestId = await _registrationService.RegisterAsync(dto);
        return Accepted(new { RequestId = requestId });
    }
}
