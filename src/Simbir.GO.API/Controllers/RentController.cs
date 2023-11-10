using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Rents;
using Simbir.GO.Application.Services;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers;

[Route("api/Rent")]
public class RentController : ApiController
{
    private readonly RentService _rentService;
    private readonly IMapper _mapper;

    public RentController(RentService rentService, IMapper mapper)
    {
        _rentService = rentService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получение транспорта доступного для аренды по параметрам
    /// </summary>
    /// <param name="params"></param>
    /// <returns></returns>
    [HttpGet("Transport")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchTransport([FromQuery]SearchTransportParams @params)
    {
        var result = await _rentService.GetRentalTransportAsync(@params);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Получение информации об аренде по id
    /// </summary>
    /// <param name="rentId"></param>
    /// <returns></returns>
    [HttpGet("{rentId:long}")]
    public async Task<IActionResult> Get(long rentId)
    {
        var result = await _rentService.GetRentByIdAsync(rentId);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(_mapper.Map<RentResponse>(result.Value)),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Получение истории аренд текущего аккаунта
    /// </summary>
    /// <returns></returns>
    [HttpGet("MyHistory")]
    public async Task<IActionResult> GetMyRentHistory()
    {
        var result = await _rentService.GetMyRentHistoryAsync();
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(_mapper.Map<List<RentResponse>>(result.Value)),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Получение истории аренд транспорта
    /// </summary>
    /// <param name="transportId"></param>
    /// <returns></returns>
    [HttpGet("TransportHistory/{transportId:long}")]
    public async Task<IActionResult> GetTransportRentHistory(long transportId)
    {
        var result = await _rentService.GetTransportRentHistoryAsync(transportId);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(_mapper.Map<List<RentResponse>>(result.Value)),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Аренда транспорта в личное пользование
    /// </summary>
    /// <param name="transportId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("New/{transportId:long}")]
    public async Task<IActionResult> StartNew(long transportId, StartRentRequest request)
    {
        var result = await _rentService.StartRentAsync(transportId, request);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Завершение аренды транспорта по id аренды
    /// </summary>
    /// <param name="rentId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("End/{rentId:long}")]
    public async Task<IActionResult> End(long rentId, EndRentRequest request)
    {
        var result = await _rentService.EndRentAsync(rentId, request);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}