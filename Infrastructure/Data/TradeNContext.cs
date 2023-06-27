using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TradeNContext : DbContext
{
    public TradeNContext(DbContextOptions<TradeNContext> options) : base(options)
    {
    }

    DbSet<User> Users { get; init; }
    DbSet<Offer> Offers { get; init; }
    DbSet<Publication> Publications { get; init; }
    DbSet<Photo> Photos { get; init; }
    DbSet<Exchange> Exchanges { get; init; }
}