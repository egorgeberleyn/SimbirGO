using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Admin.Rents;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers.Admin;

[Route("api/Admin")]
[Authorize(Roles = "Admin")]
public class AdminRentController : ApiController
{
    private readonly IAdminRentService _adminRentService;

    public AdminRentController(IAdminRentService adminRentService)
    {
        _adminRentService = adminRentService;
    }

    /// <summary>
    /// Получение информации по аренде по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Rent/{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var result = await _adminRentService.GetRentByIdAsync(id);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Получение истории аренд пользователя с id={userId}
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("UserHistory/{userId:long}")]
    public async Task<IActionResult> GetUserHistory(long userId)
    {
        var result = await _adminRentService.GetUserRentHistoryAsync(userId);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Получение истории аренд транспорта с id={transportId}
    /// </summary>
    /// <param name="transportId"></param>
    /// <returns></returns>
    [HttpGet("TransportHistory{transportId:long}")]
    public async Task<IActionResult> GetTransportHistory(long transportId)
    {
        var result = await _adminRentService.GetTransportRentHistoryAsync(transportId);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Создание новой аренды
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Rent")]
    public async Task<IActionResult> Create(CreateRentRequest request)
    {
        var result = await _adminRentService.CreateRentAsync(request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Завершение аренды транспорта по id аренды
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Rent/End/{id:long}")]
    public async Task<IActionResult> End(long id, AdminEndRentRequest request)
    {
        var result = await _adminRentService.AdminEndRentAsync(id, request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Изменение записи об аренде по id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Rent/{id:long}")]
    public async Task<IActionResult> Update(long id, UpdateRentRequest request)
    {
        var result = await _adminRentService.UpdateRentAsync(id, request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Удаление информации об аренде по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("Rent/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _adminRentService.DeleteRentAsync(id);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}