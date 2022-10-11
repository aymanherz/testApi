using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace testApi.Models
{
    public class BeerRateContext : DbContext
    {
        public BeerRateContext(DbContextOptions<BeerRateContext> options) 
            : base(options) 
        {
        }
        public DbSet<BeerRate> Beers { get; set; } = null!;
    }
}
