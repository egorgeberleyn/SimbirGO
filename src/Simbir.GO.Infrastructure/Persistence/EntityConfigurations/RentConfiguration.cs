using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simbir.GO.Domain.Rents;
using Simbir.GO.Domain.Rents.Enums;

namespace Simbir.GO.Infrastructure.Persistence.EntityConfigurations;

public class RentConfiguration : IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.ToTable("rents");
        builder.HasKey(r => r.Id);
        
        builder.Property(p => p.PriceType).IsRequired()
            .HasConversion(
                p => p.ToString(), 
                p => (PriceType)Enum.Parse(typeof(PriceType), p));
    }
}