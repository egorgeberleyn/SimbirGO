using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Rents.Enums;
using Simbir.GO.Domain.Transports.Enums;
using Simbir.GO.Infrastructure.Auth;
using Simbir.GO.Infrastructure.Auth.Utils;
using Simbir.GO.Infrastructure.Persistence;
using Simbir.GO.Infrastructure.Persistence.Repositories;
using Simbir.GO.Infrastructure.Utils;
using Simbir.GO.Shared.Persistence.Repositories;

namespace Simbir.GO.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddAuth(configuration);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        return services;
    }
    
    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Postgres"));
        //Map C# enum -> postgres enum
        dataSourceBuilder.MapEnum<Role>();
        dataSourceBuilder.MapEnum<TransportType>();
        dataSourceBuilder.MapEnum<PriceType>();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddScoped<IAppDbContext>(factory => factory.GetRequiredService<AppDbContext>());
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });

        services.AddScoped<ITransportRepository, TransportRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRentRepository, RentRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddAuth(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, CurrentUserContext>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

        return services;
    }
}