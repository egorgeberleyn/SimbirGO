using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Enums;

namespace Simbir.GO.Infrastructure.Persistence.EntityConfigurations;

public class TransportConfiguration : IEntityTypeConfiguration<Transport>
{
    public void Configure(EntityTypeBuilder<Transport> builder)
    {
        builder.ToTable("transports");
        builder.HasKey(t => t.Id);
        builder.Property(p => p.TransportType).IsRequired()
            .HasConversion(
                p => p.ToString(), 
                p => (TransportType)Enum.Parse(typeof(TransportType), p));
        builder.OwnsOne(t => t.Coordinate);
    }
}