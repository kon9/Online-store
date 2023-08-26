using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.ApiService.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class HealthCheckController : Controller
{
    [HttpGet]
    public string Check() => "Service is online";
}

