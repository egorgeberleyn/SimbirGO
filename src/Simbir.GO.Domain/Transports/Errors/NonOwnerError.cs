using FluentResults;

namespace Simbir.GO.Domain.Transports.Errors;

public class NonOwnerError : Error
{
    public NonOwnerError() : base("The user is not the owner of the transport")
    {
        Metadata.Add("ErrorCode", 400);
    }
}