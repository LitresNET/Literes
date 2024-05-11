using System.Reflection;
using System.Text.Json;
using Litres.Domain.Entities;
using Litres.Infrastructure.Configurations.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure;

public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options, 
        IConfiguration configuration) 
    : IdentityDbContext<User, IdentityRole<long>, long>(options)
{
    public DbSet<Author> Author { get; set; }
    public DbSet<Book> Book { get; set; }
    public DbSet<Contract> Contract { get; set; }
    public DbSet<ExternalService> ExternalService { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<PickupPoint> PickupPoint { get; set; }
    public DbSet<Publisher> Publisher { get; set; }
    public DbSet<Request> Request { get; set; }
    public DbSet<Review> Review { get; set; }
    public DbSet<ReviewLike> ReviewLike { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<Subscription> Subscription { get; set; }
    public DbSet<User> User { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PublisherEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        
        ConfigureMicrosoftIdentityRelations(modelBuilder);
        
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var assembly = Assembly.Load("Litres.Domain");
        var rootPath = configuration.GetValue<string>(WebHostDefaults.ContentRootKey) ?? "";
        var path = Path.Combine(rootPath, "seedConfig.json");
        var jsonString = File.ReadAllText(path);
        
        var classes = JsonSerializer.Deserialize<Dictionary<string, JsonElement[]>>(jsonString)!;
        
        foreach (var c in classes)
        {
            var typeName = $"Litres.Domain.Entities.{c.Key}";
            var type = assembly.GetType(typeName);
            var objects = c.Value.Select(element => element.Deserialize(type)).ToList();
            
            modelBuilder.Entity(type).HasData(objects);
        }
    }

    private static void ConfigureMicrosoftIdentityRelations(ModelBuilder modelBuilder)
    {
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
        modelBuilder.Entity<IdentityUserLogin<long>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey }); 
        
        modelBuilder.Entity<IdentityUserClaim<long>>()
            .HasKey(uc => uc.Id);
        
        modelBuilder.Entity<IdentityUserRole<long>>()
            .HasKey(r => new { r.UserId, r.RoleId });
        
        modelBuilder.Entity<IdentityRole<long>>(b =>
        {
            b.HasMany<IdentityUserRole<long>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            b.HasMany<IdentityRoleClaim<long>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        });
        
        modelBuilder.Entity<IdentityUserToken<long>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
    }
}