namespace Simbir.GO.Application.Interfaces;

public interface IPaymentService
{
    Task PaymentHesoyamAsync(long accountId);
}