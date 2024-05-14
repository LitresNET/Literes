using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Litres.Infrastructure.Configurations.EntityConfigurations;

public class PublisherEntityConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder
            .HasOne(p => p.User).WithOne()
            .HasForeignKey<Publisher>(p => p.UserId);
    }
}