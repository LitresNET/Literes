using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Litres.Data.Configurations.EntityConfigurations;

public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .HasMany(e => e.Books)
            .WithMany(e => e.Orders)
            .UsingEntity<BookOrder>( 
                j => j.HasOne(bo => bo.Book).WithMany(b => b.BookOrders)
                    .HasForeignKey(bo => bo.BookId),
                j => j.HasOne(bo => bo.Order).WithMany(o => o.OrderedBooks)
                    .HasForeignKey(bo => bo.OrderId),
                j => j.HasKey(bo => bo.Id));
    }
}