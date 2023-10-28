using Microsoft.Extensions.DependencyInjection;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Services;

namespace Simbir.GO.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IRentService, RentService>();
        services.AddScoped<ITransportService, TransportService>();
        
        return services;
    }
}