using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Infrastructure.Persistence.EntityConfigurations;

public class TransportConfiguration : IEntityTypeConfiguration<Transport>
{
    public void Configure(EntityTypeBuilder<Transport> builder)
    {
        builder.ToTable("transports");
        builder.HasKey(t => t.Id);
        builder.OwnsOne(t => t.Coordinate);
    }
}