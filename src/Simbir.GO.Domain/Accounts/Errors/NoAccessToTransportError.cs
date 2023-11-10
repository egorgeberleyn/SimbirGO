using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class NoAccessToTransportError : Error
{
    public NoAccessToTransportError() : base("User is not the owner of the transport")
    {
        Metadata.Add("ErrorCode", 400);
    }
}