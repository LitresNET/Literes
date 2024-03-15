using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend;

public class ApplicationDbContext : DbContext
{
    public DbSet<Author> Author { get; set; }
    public DbSet<Book> Book { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<CommentLike> CommentLike { get; set; }
    public DbSet<Contract> Contract { get; set; }
    public DbSet<ExternalService> ExternalService { get; set; }
    public DbSet<Genre> Genre { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<PickupPoint> PickupPoint { get; set; }
    public DbSet<Publisher> Publisher { get; set; }
    public DbSet<Review> Review { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<Subscription> Subscription { get; set; }
    public DbSet<User> User { get; set; }

    public ApplicationDbContext(){}
    
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(e => e.Purchased)
            .WithMany(e => e.Purchesed)
            .UsingEntity("Purchesed",
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
            

        modelBuilder.Entity<Author>()
            .HasMany(e => e.Books)
            .WithOne(e => e.Author)
            .HasForeignKey(e => e.AuthorId);
        
        modelBuilder.Entity<Author>()
            .HasMany(e => e.Series)
            .WithOne(e => e.Author)
            .HasForeignKey(e => e.AuthorId);
        
        modelBuilder.Entity<Book>()
            .HasMany(e => e.BookGenres)
            .WithMany(e => e.BookGenres)
            .UsingEntity("BookGenres", 
                l => l.HasOne(typeof(Genre)).WithMany().HasForeignKey("GenreId").HasPrincipalKey(nameof(Models.Genre.Id)),
                r => r.HasOne(typeof(Book)).WithMany().HasForeignKey("BookId").HasPrincipalKey(nameof(Models.Book.Id)),
                j => j.HasKey("BookId", "GenreId"));;

        modelBuilder.Entity<Series>()
            .HasMany(e => e.Books)
            .WithOne(e => e.Series)
            .HasForeignKey(e => e.SeriesId);
                                                                            
        
        modelBuilder.Entity<Publisher>()
            .HasMany(e => e.Books)
            .WithOne(e => e.Publisher)
            .HasForeignKey(e => e.PublisherId);
        
        modelBuilder.Entity<Review>()
            .HasMany(e => e.Comments)
            .WithOne(e => e.Review)
            .HasForeignKey(e => e.ReviewId);
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.Comments)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
        
        modelBuilder.Entity<Comment>()
            .HasMany(e => e.CommentLikes)
            .WithOne(e => e.Comment)
            .HasForeignKey(e => e.CommentId);
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.CommentLikes)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.ExternalServices)
            .WithMany(e => e.Users)
            .UsingEntity("UserExternalServices", 
                l => l.HasOne(typeof(ExternalService)).WithMany().HasForeignKey("ExternalServiceId").HasPrincipalKey(nameof(Models.ExternalService.Id)),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(Models.User.Id)),
                j => j.HasKey("UserId", "ExternalServiceId"));;
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.Orders)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
        
        modelBuilder.Entity<PickupPoint>()
            .HasMany(e => e.Orders)
            .WithOne(e => e.PickupPoint)
            .HasForeignKey(e => e.PickupPointId);
        
        modelBuilder.Entity<Contract>()
            .HasOne(e => e.Publisher)
            .WithOne(e => e.Contract)
            .HasForeignKey<Publisher>(e => e.ContractId)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasOne(e => e.Publisher)
            .WithOne(e => e.User)
            .HasForeignKey<Publisher>(e => e.UserId);
        
        modelBuilder.Entity<Subscription>()
            .HasMany(e => e.Users)
            .WithOne(e => e.Subscription)
            .HasForeignKey(e => e.SubscriptionId);

        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}