using System.Text.Json;
using Litres.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Litres.Data.Configurations;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    private IConfiguration _configuration;
    
    public DbSet<Author> Author { get; set; }
    public DbSet<Book> Book { get; set; }
    public DbSet<ReviewLike> ReviewLike { get; set; }
    public DbSet<Contract> Contract { get; set; }
    public DbSet<ExternalService> ExternalService { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<PickupPoint> PickupPoint { get; set; }
    public DbSet<Publisher> Publisher { get; set; }
    public DbSet<Review> Review { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<Subscription> Subscription { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Request> Request { get; set; }

    public ApplicationDbContext(){}
    
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    
    public ApplicationDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Wallet)
            .HasPrecision(18, 4);
        
        
        // TODO: в конфиг
        #region Relationships
        
        modelBuilder.Entity<Publisher>().ToTable("Publisher");

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId);

        modelBuilder.Entity<User>()
            .HasMany(e => e.Purchased)
            .WithMany(e => e.Purchased)
            .UsingEntity("Purchased",
                l => l.HasOne(typeof(Book)).WithMany().HasForeignKey("BookId").HasPrincipalKey(nameof(Litres.Data.Models.Book.Id)),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(Litres.Data.Models.User.Id)),
                j => j.HasKey("UserId", "BookId"));;

        modelBuilder.Entity<User>()
            .HasMany(e => e.Favourites)
            .WithMany(e => e.Favourites)
            .UsingEntity("Favourites", 
                l => l.HasOne(typeof(Book)).WithMany().HasForeignKey("BookId").HasPrincipalKey(nameof(Litres.Data.Models.Book.Id)),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(Litres.Data.Models.User.Id)),
                j => j.HasKey("UserId", "BookId"));;
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.ExternalServices)
            .WithMany(e => e.Users)
            .UsingEntity("UserExternalServices", 
                l => l.HasOne(typeof(ExternalService)).WithMany().HasForeignKey("ExternalServiceId").HasPrincipalKey(nameof(Litres.Data.Models.ExternalService.Id)),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(Litres.Data.Models.User.Id)),
                j => j.HasKey("UserId", "ExternalServiceId"));;
        
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Subscription)
            .WithMany(s => s.Users);

        modelBuilder.Entity<Publisher>()
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Publisher>(p => p.UserId);
        
        //Связи, которые сгенерировал chatgpt. Вставил на пох, просто чтобы работало
        //TODO: исправить (для аутентификации и авторизации)
        modelBuilder.Entity<IdentityUserLogin<long>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey }); 
        modelBuilder.Entity<IdentityUserRole<long>>()
            .HasKey(r => new { r.UserId, r.RoleId });
        modelBuilder.Entity<IdentityUserToken<long>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        //Исправить исправить исправить
        
        #endregion

        #region DefaulValue
        
        modelBuilder.Entity<User>()
            .Property(u => u.AvatarUrl)
            .HasDefaultValue("/"); //TODO: ссылка на дефолтную аватарку
        modelBuilder.Entity<User>()
            .Property(u => u.SubscriptionId)
            .HasDefaultValue((long?) 1);
        
        #endregion
        
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var rootPath = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) ?? "";
        var path = Path.Combine(rootPath, "seedConfig.json");
        var jsonString = File.ReadAllText(path);
        
        var classes = JsonSerializer.Deserialize<Dictionary<string, JsonElement[]>>(jsonString)!;
        
        foreach (var c in classes)
        {
            var type = Type.GetType($"Litres.Data.Models.{c.Key}");
            var objects = c.Value.Select(element => element.Deserialize(type)).ToList();
            
            modelBuilder.Entity(type).HasData(objects);
        }
    }
}