using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simbir.GO.Domain.Rents;

namespace Simbir.GO.Infrastructure.Persistence.EntityConfigurations;

public class RentConfiguration : IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.ToTable("rents");
        builder.HasKey(r => r.Id);
    }
}