using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Simbir.GO.Shared.Presentation;

[ApiController]
[Authorize]
public class ApiController : Controller
{
    protected IActionResult Problem(List<IError> errors)
    {
        if (errors.Count == 0)
            return Problem();

        var statusStr = errors[0].Metadata["ErrorCode"].ToString();
        if (statusStr is null)
            throw new ArgumentNullException(nameof(statusStr), "ErrorCode must have a code status value");
        
        if(!int.TryParse(statusStr, out var statusCode))
            throw new InvalidCastException("ErrorCode must be of numeric type");
            
        return Problem(statusCode: statusCode, title: errors[0].Message);
    }
}