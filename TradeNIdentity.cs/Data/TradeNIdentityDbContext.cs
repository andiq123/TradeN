using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TradeNIdentity.cs.Data;

public class TradeNIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public TradeNIdentityDbContext(DbContextOptions<TradeNIdentityDbContext> options) : base(options)
    {
    }
}