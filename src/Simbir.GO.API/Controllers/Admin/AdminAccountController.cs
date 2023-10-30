using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Admin.Accounts;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers.Admin;

[Route("Admin/Account")]
[Authorize(Roles="Admin")]
public class AdminAccountController : ApiController
{
    private readonly IAdminAccountService _adminAccountService;

    public AdminAccountController(IAdminAccountService adminAccountService)
    {
        _adminAccountService = adminAccountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts(int start, int count)
    {
        var result = await _adminAccountService.GetAccountsAsync(start, count);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetAccount(long id)
    {
        var result = await _adminAccountService.GetAccountAsync(id);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
    {
        var result = await _adminAccountService.CreateAccountAsync(request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAccount(long id, UpdateAccountRequest request)
    {
        var result = await _adminAccountService.UpdateAccountAsync(id, request);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> DeleteAccount(long id)
    {
        var result = await _adminAccountService.DeleteAccountAsync(id);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}