using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Simbir.GO.API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    [Route("/error")]
    protected IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        
        /*Log("Code {StatusCode}, Status: {Status} : {Message} ({Description}) StackTrace: {StackTrace}", 
            code, status, message, desc, exception?.StackTrace);*/

        return Problem(statusCode: 500, title: exception?.Message);
    }
}