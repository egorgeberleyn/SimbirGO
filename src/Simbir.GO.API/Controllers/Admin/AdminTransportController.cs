using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Admin.Transports;
using Simbir.GO.Application.Services.Admin;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers.Admin;

[Route("api/Admin/Transport")]
[Authorize(Roles = "Admin")]
public class AdminTransportController : ApiController
{
    private readonly AdminTransportService _adminTransportService;

    public AdminTransportController(AdminTransportService adminTransportService)
    {
        _adminTransportService = adminTransportService;
    }

    /// <summary>
    /// Получение списка всех транспортных средств
    /// </summary>
    /// <param name="params"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]SelectTransportParams @params)
    {
        var result = await _adminTransportService.GetTransportsAsync(@params);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Получение информации о транспортном средстве по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var result = await _adminTransportService.GetTransportAsync(id);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Создание нового транспортного средства
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateTransportRequest request)
    {
        var result = await _adminTransportService.CreateTransportAsync(request);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Изменение транспортного средства по id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, AdminUpdateTransportRequest request)
    {
        var result = await _adminTransportService.UpdateTransportAsync(id, request);
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
        var result = await _adminTransportService.DeleteTransportAsync(id);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}