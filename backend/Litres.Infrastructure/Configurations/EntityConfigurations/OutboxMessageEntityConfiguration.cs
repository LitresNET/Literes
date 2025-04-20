using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Litres.Infrastructure.Configurations.EntityConfigurations;

public class OutboxMessageEntityConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Guid);
        builder.Property(x => x.Content)
            .HasColumnType("nvarchar(max)");
        
        
        builder.Property(m => m.OccuredOn)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => v.ToLocalTime());
    }
}