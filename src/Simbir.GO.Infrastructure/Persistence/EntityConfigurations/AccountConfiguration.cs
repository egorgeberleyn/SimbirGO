using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Accounts.Enums;

namespace Simbir.GO.Infrastructure.Persistence.EntityConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");
        builder.HasKey(a => a.Id);
        builder.OwnsOne(a => a.Balance);
        
        builder.Property(p => p.Role).IsRequired()
            .HasConversion(
                p => p.ToString(), 
                p => (Role)Enum.Parse(typeof(Role), p));
    }
}