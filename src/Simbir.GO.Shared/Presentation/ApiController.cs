using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Simbir.GO.Shared.Presentation;

[ApiController]
[Authorize]
[Route("api")]
public class ApiController : Controller
{
    
}