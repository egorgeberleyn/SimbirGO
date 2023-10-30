using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Rents;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers;

[Route("api/Rent")]
public class RentController : ApiController
{
    private readonly IRentService _rentService;

    public RentController(IRentService rentService)
    {
        _rentService = rentService;
    }

    [HttpGet("Transport")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRentalTransport([FromQuery]SearchTransportParams @params)
    {
        var result = await _rentService.GetRentalTransportAsync(@params);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpGet("{rentId:long}")]
    public async Task<IActionResult> GetRent(long rentId)
    {
        var result = await _rentService.GetRentByIdAsync(rentId);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpGet("MyHistory")]
    public async Task<IActionResult> GetMyRentHistory()
    {
        var result = await _rentService.GetMyRentHistoryAsync();
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpGet("TransportHistory/{transportId:long}")]
    public async Task<IActionResult> GetTransportRentHistory(long transportId)
    {
        var result = await _rentService.GetTransportRentHistoryAsync(transportId);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPost("New/{transportId:long}")]
    public async Task<IActionResult> StartNewRent(long transportId, StartRentRequest request)
    {
        var result = await _rentService.StartRentAsync(transportId, request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPost("End/{rentId:long}")]
    public async Task<IActionResult> StartNewRent(long rentId, EndRentRequest request)
    {
        var result = await _rentService.EndRentAsync(rentId, request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}