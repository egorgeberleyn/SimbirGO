using FluentResults;

namespace Simbir.GO.Application.Interfaces;

public interface IPaymentService
{
    Task<Result<Success>> PaymentHesoyamAsync(long accountId);
}