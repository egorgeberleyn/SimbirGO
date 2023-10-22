using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simbir.GO.Domain.Accounts;

namespace Simbir.GO.Infrastructure.Persistence.EntityConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");
        builder.HasKey(a => a.Id);
        builder.OwnsOne(a => a.Balance);
    }
}