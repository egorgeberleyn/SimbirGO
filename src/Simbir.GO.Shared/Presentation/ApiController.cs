using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Simbir.GO.Shared.Presentation;

[ApiController]
[Authorize]
public class ApiController : Controller
{
    
}