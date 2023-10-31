using Mapster;
using Simbir.GO.Application.Contracts.Accounts;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Accounts.ValueObjects;

namespace Simbir.GO.API.Mapping;

public class AccountMappingConfig  : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Account, AccountResponse>()
            .MapToConstructor(true);

        config.ForType<Balance, BalanceResponse>()
            .MapToConstructor(true);
    }
}