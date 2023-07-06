using Application.Contracts.Cloudinary;
using Application.Contracts.Persistence;
using Infrastructure.Data;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
        
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        
        services.AddDbContext<TradeNContext>(x => x.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}