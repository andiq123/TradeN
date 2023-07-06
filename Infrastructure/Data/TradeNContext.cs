using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TradeNContext : DbContext
{
    public TradeNContext(DbContextOptions<TradeNContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; init; }
    public DbSet<Offer> Offers { get; init; }
    public DbSet<Publication> Publications { get; init; }
    public DbSet<Photo> Photos { get; init; }
    public DbSet<Exchange> Exchanges { get; init; }
}