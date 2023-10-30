using FluentResults;

namespace Simbir.GO.Domain.Rents.Errors;

public class NoAccessToRentError : Error
{
    public NoAccessToRentError() : base("User is not the owner of the transport or the lessee")
    {
        Metadata.Add("ErrorCode", 400);
    }
}