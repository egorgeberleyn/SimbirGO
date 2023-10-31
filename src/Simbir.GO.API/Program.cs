using Simbir.GO.API;
using Simbir.GO.API.Middlewares;
using Simbir.GO.Application;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Infrastructure;
using Simbir.GO.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddShared();

var app = builder.Build();
{
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    
    app.UseExceptionHandler("/error");
    
    using (var scope = app.Services.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        var jwtTokenGenerator = scopedProvider.GetRequiredService<IJwtTokenGenerator>();
        app.UseMiddleware<JwtInvalidationMiddleware>(jwtTokenGenerator);
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}