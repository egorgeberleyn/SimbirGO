using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Services;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers;

public class PaymentController : ApiController
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Добавляет на баланс аккаунта с id={accountId} 250 000 денежных единиц
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    [HttpPost("{accountId:long}")]
    public async Task<IActionResult> Hesoyam(long accountId)
    {
        var result = await _paymentService.PaymentHesoyamAsync(accountId);
        return result switch {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}