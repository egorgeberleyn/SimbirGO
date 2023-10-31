using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Contracts.Accounts;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers;

[Route("api/Account")]
public class AccountController : ApiController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Получение данных о текущем аккаунте
    /// </summary>
    /// <returns></returns>
    [HttpGet("Me")]
    public async Task<IActionResult> GetCurrent()
    {
        var result = await _accountService.GetCurrentAccountAsync();
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Получение нового jwt токена пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("SignIn")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(SignInAccountRequest request)
    {
        var authResult = await _accountService.SignInAsync(request);
        return authResult switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(authResult.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Регистрация нового аккаунта
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("SignUp")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(SignUpAccountRequest request)
    {
        var authResult = await _accountService.SignUpAsync(request);
        return authResult switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(authResult.Value),
            _ => NoContent()
        };
    }
    
    /// <summary>
    /// Выход из аккаунта
    /// </summary>
    /// <returns></returns>
    [HttpPost("SignOut")]
    public new async Task<IActionResult> SignOut()
    {
        await _accountService.SignOutAsync();
        return Ok();
    }
    
    /// <summary>
    /// Обновление своего аккаунта
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Update")]
    public new async Task<IActionResult> Update(UpdateAccountRequest request)
    {
        var updateAccountResult = await _accountService.UpdateAccountAsync(request);
        return updateAccountResult switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(updateAccountResult.Value),
            _ => NoContent()
        };
    }
}