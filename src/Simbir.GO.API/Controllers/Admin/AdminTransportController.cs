using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Admin.Transports;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers.Admin;

[Route("api/Admin/Transport")]
[Authorize(Roles = "Admin")]
public class AdminTransportController : ApiController
{
    private readonly IAdminTransportService _adminTransportService;

    public AdminTransportController(IAdminTransportService adminTransportService)
    {
        _adminTransportService = adminTransportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]SelectTransportParams @params)
    {
        var result = await _adminTransportService.GetTransportsAsync(@params);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var result = await _adminTransportService.GetTransportAsync(id);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateTransportRequest request)
    {
        var result = await _adminTransportService.CreateTransportAsync(request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UpdateTransportRequest request)
    {
        var result = await _adminTransportService.UpdateTransportAsync(id, request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _adminTransportService.DeleteTransportAsync(id);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}