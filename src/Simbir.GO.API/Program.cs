using Simbir.GO.API;
using Simbir.GO.API.Middlewares;
using Simbir.GO.Application;
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
    
    app.UseMiddleware<JwtInvalidationMiddleware>();
    
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}