namespace Checkout.DataAccess.DbContexts;

using Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

public class CheckoutDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public CheckoutDbContext(DbContextOptions<CheckoutDbContext> options, IConfiguration configuration)
        : base(options)
    {
        Configuration = configuration;
    }

    public CheckoutDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public DbSet<Item> Items { get; set; }

    public DbSet<Basket> Baskets { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("CheckoutDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        modelBuilder.ApplyConfiguration(new BasketConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}