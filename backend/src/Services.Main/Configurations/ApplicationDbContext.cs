using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Configurations;

public class ApplicationDbContext : DbContext
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
    }
}