using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Accounts;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers;

public class AccountController : ApiController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("Me")]
    public async Task<IActionResult> GetCurrentAccount()
    {
        var result = await _accountService.GetCurrentAccountAsync();
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> SignIn(SignInAccountRequest request)
    {
        var authResult = await _accountService.SignInAsync(request);
        return authResult switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(authResult.Value),
            _ => NoContent()
        };
    }
    
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpAccountRequest request)
    {
        var authResult = await _accountService.SignUpAsync(request);
        return authResult switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(authResult.Value),
            _ => NoContent()
        };
    }
    
    [HttpPost("SignOut")]
    public new async Task<IActionResult> SignOut()
    {
        await _accountService.SignOutAsync();
        return Ok();
    }
    
    [HttpPut("Update")]
    public new async Task<IActionResult> UpdateAccount(UpdateAccountRequest request)
    {
        var updateAccountResult = await _accountService.UpdateAccountAsync(request);
        return updateAccountResult switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(updateAccountResult.Value),
            _ => NoContent()
        };
    }
}