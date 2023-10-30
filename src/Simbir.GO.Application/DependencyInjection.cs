﻿using Microsoft.Extensions.DependencyInjection;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Services;
using Simbir.GO.Application.Services.Admin;
using Simbir.GO.Domain.Transports.Services;

namespace Simbir.GO.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<LocationFinder>();
        
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IRentService, RentService>();
        services.AddScoped<ITransportService, TransportService>();

        services.AddScoped<IAdminAccountService, AdminAccountService>();
        services.AddScoped<IAdminTransportService, AdminTransportService>();
        services.AddScoped<IAdminRentService, AdminRentService>();
        
        return services;
    }
}