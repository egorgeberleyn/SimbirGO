using Mapster;
using Simbir.GO.Application.Contracts.Rents;
using Simbir.GO.Domain.Rents;

namespace Simbir.GO.API.Mapping;

public class RentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Rent, RentResponse>()
            .MapToConstructor(true);
    }
}