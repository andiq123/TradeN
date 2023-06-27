using System.Reflection;
using Application.Features.Exchanges;
using Application.Features.Offers;
using Application.Features.Publications;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(ApplicationAssembly)));
        services.AddScoped<PublicationService>();
        services.AddScoped<OffersService>();
        services.AddScoped<ExchangeService>();
        return services;
    }
}