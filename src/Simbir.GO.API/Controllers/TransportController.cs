using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Transports;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers;

[Route("api/Transport")]
public class TransportController : ApiController
{
    private readonly ITransportService _transportService;

    public TransportController(ITransportService transportService)
    {
        _transportService = transportService;
    }

    /// <summary>
    /// Получение информации о транспорте по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(long id)
    {
        var result = await _transportService.GetTransportByIdAsync(id);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Добавление нового транспорта
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Add(AddTransportRequest request)
    {
        var result = await _transportService.AddTransportAsync(request);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Изменение транспорта по id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UpdateTransportRequest request)
    {
        var result = await _transportService.UpdateTransportAsync(id, request);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Удаление транспорта по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _transportService.DeleteTransportAsync(id);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}