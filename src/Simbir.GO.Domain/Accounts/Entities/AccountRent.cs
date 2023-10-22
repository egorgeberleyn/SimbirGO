using Simbir.GO.Domain.Transports;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Accounts.Entities;

public class AccountRent : Entity
{
    public long AccountId { get; init; }
    public long TransportId { get; init; }

    public Account Account { get; init; } = null!;
    public Transport Transport { get; init; } = null!;

    public AccountRent(long accountId, long transportId)
    {
        AccountId = accountId;
        TransportId = transportId;
    }
}