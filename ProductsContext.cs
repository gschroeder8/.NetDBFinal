
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

public class ProductContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration =  new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json");
            
        var config = configuration.Build();
        optionsBuilder.UseSqlServer(@config["NW_Traders:ConnectionString"]);
    }
}