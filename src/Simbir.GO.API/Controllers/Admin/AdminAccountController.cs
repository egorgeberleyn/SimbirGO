using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Admin.Accounts;
using Simbir.GO.Application.Services.Admin;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers.Admin;

[Route("Admin/Account")]
[Authorize(Roles="Admin")]
public class AdminAccountController : ApiController
{
    private readonly AdminAccountService _adminAccountService;

    public AdminAccountController(AdminAccountService adminAccountService)
    {
        _adminAccountService = adminAccountService;
    }

    /// <summary>
    /// Получение списка всех аккаунтов
    /// </summary>
    /// <param name="start"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(int start, int count)
    {
        var result = await _adminAccountService.GetAccountsAsync(start, count);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Получение информации об аккаунте по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var result = await _adminAccountService.GetAccountAsync(id);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Создание администратором нового аккаунта
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountRequest request)
    {
        var result = await _adminAccountService.CreateAccountAsync(request);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Изменение администратором аккаунта по id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, AdminUpdateAccountRequest request)
    {
        var result = await _adminAccountService.UpdateAccountAsync(id, request);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Удаление аккаунта по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _adminAccountService.DeleteAccountAsync(id);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}