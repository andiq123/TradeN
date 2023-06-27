using System.Text.Json.Serialization;
using API.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
{
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    builder.Services.AddServices(configuration);
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