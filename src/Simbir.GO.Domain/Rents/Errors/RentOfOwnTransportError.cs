using FluentResults;

namespace Simbir.GO.Domain.Rents.Errors;

public class RentOfOwnTransportError : Error
{
    public RentOfOwnTransportError() : base("Renting your own transport is not possible")
    {
        Metadata.Add("ErrorCode", 400);
    }
}