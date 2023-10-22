using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simbir.GO.Domain.Accounts.Entities;

namespace Simbir.GO.Infrastructure.Persistence.EntityConfigurations;

public class AccountRentConfiguration : IEntityTypeConfiguration<AccountRent>
{
    public void Configure(EntityTypeBuilder<AccountRent> builder)
    {
        builder.ToTable("account_rents");
        builder.HasKey(a => new { a.AccountId, a.TransportId });

        builder.HasOne(r => r.Account)
            .WithMany(a => a.AccountRents)
            .HasForeignKey(r => r.AccountId);
    }
}