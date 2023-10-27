using Microsoft.AspNetCore.Mvc;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Shared.Presentation;

namespace Simbir.GO.API.Controllers;

public class PaymentController : ApiController
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("{accountId:long}")]
    public async Task<IActionResult> PaymentHesoyam(long accountId)
    {
        var result = await _paymentService.PaymentHesoyamAsync(accountId);
        return result switch {
            { IsFailed: true } => Problem(),
            { IsSuccess: true } => Ok(result.Value),
            _ => NoContent()
        };
    }
}