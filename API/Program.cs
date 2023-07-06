using System.Text.Json.Serialization;
using API.Extensions;
using API.Middleware;
using API.Seed;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradeNIdentity.cs.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
{
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    builder.Services.AddServices(configuration);
}

//seeding data get the scope
using var scope = builder.Services.BuildServiceProvider().CreateScope();
var services = scope.ServiceProvider;
try
{
    await Seeder.Seed(services);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();


{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    await app.RunAsync();
}