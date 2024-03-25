using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Configurations;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public DbSet<Author> Author { get; set; }
    public DbSet<Book> Book { get; set; }
    public DbSet<Comment> Comment { get; set; }
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
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Связи
        modelBuilder.Entity<User>()
            .HasMany(e => e.Purchased)
            .WithMany(e => e.Purchased)
            .UsingEntity("Purchased",
                l => l.HasOne(typeof(Book)).WithMany().HasForeignKey("BookId").HasPrincipalKey(nameof(Models.Book.Id)),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(Models.User.Id)),
                j => j.HasKey("UserId", "BookId"));;

        modelBuilder.Entity<User>()
            .HasMany(e => e.Favourites)
            .WithMany(e => e.Favourites)
            .UsingEntity("Favourites", 
                l => l.HasOne(typeof(Book)).WithMany().HasForeignKey("BookId").HasPrincipalKey(nameof(Models.Book.Id)),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(Models.User.Id)),
                j => j.HasKey("UserId", "BookId"));;
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.ExternalServices)
            .WithMany(e => e.Users)
            .UsingEntity("UserExternalServices", 
                l => l.HasOne(typeof(ExternalService)).WithMany().HasForeignKey("ExternalServiceId").HasPrincipalKey(nameof(Models.ExternalService.Id)),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(Models.User.Id)),
                j => j.HasKey("UserId", "ExternalServiceId"));;
        
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Subscription)
            .WithMany(s => s.Users);
        
        
        //Связи, которые сгенерировал chatgpt. Вставил на пох, просто чтобы работало TODO: исправить (для аутентификации и авторизации)
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
            .Property(u => u.AvaterUrl)
            .HasDefaultValue("/"); //TODO: ссылка на дефолтную аватарку
        modelBuilder.Entity<User>()
            .Property(u => u.SubscriptionId)
            .HasDefaultValue("1");
        #endregion
        
        #region HasData
        modelBuilder.Entity<Subscription>().HasData(new Subscription
        {
            Id = 1,
            Type = SubscriptionType.Free,
            Price = 0,
        }); //TODO: добавить ещё HasData на subscription и на другие типы
        #endregion
        
    }
}