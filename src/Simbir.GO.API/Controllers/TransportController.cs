using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Transports;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers;

public class TransportController : ApiController
{
    private readonly ITransportService _transportService;

    public TransportController(ITransportService transportService)
    {
        _transportService = transportService;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetTransport(long id)
    {
        var result = await _transportService.GetTransportByIdAsync(id);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPost]
    public async Task<IActionResult> AddTransport(AddTransportRequest request)
    {
        var result = await _transportService.AddTransportAsync(request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateTransport(long id, UpdateTransportRequest request)
    {
        var result = await _transportService.UpdateTransportAsync(id, request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteTransport(long id)
    {
        var result = await _transportService.DeleteTransportAsync(id);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}