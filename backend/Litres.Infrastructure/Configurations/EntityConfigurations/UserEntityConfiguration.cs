using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Litres.Infrastructure.Configurations.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(u => u.Wallet)
            .HasPrecision(18, 4);
        
        builder
            .HasOne(u => u.Subscription)
            .WithMany(s => s.Users);
        
        builder
            .Property(u => u.AvatarUrl)
            .HasDefaultValue("/");
        builder
            .Property(u => u.SubscriptionId)
            .HasDefaultValue(1L);
        
        builder
            .HasMany(e => e.Purchased)
            .WithMany(e => e.Purchased)
            .UsingEntity("Purchased",
                l => l.HasOne(typeof(Book)).WithMany()
                    .HasForeignKey("BookId").HasPrincipalKey(nameof(Book.Id)),
                r => r.HasOne(typeof(User)).WithMany()
                    .HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
                j => j.HasKey("UserId", "BookId"));

        builder
            .HasMany(e => e.Favourites)
            .WithMany(e => e.Favourites)
            .UsingEntity("Favourites", 
                l => l.HasOne(typeof(Book)).WithMany()
                    .HasForeignKey("BookId").HasPrincipalKey(nameof(Book.Id)),
                r => r.HasOne(typeof(User)).WithMany()
                    .HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
                j => j.HasKey("UserId", "BookId"));
        
        builder
            .HasMany(e => e.ExternalServices)
            .WithMany(e => e.Users)
            .UsingEntity("UserExternalServices", 
                l => l.HasOne(typeof(ExternalService)).WithMany()
                    .HasForeignKey("ExternalServiceId").HasPrincipalKey(nameof(ExternalService.Id)),
                r => r.HasOne(typeof(User)).WithMany()
                    .HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
                j => j.HasKey("UserId", "ExternalServiceId"));

        #region Microsoft identity integration settings

        builder.HasMany(u => u.Roles).WithMany();
                
        builder.HasMany(u => u.Logins).WithOne()
            .HasForeignKey(ul => ul.UserId).IsRequired();
        
        builder.HasMany(u => u.Claims).WithOne()
            .HasForeignKey(uc => uc.UserId).IsRequired();
        
        builder.HasMany(u => u.Tokens).WithOne()
            .HasForeignKey(ut => ut.UserId).IsRequired();

        #endregion
    }
}