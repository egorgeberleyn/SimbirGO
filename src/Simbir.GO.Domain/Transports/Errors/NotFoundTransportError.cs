using FluentResults;

namespace Simbir.GO.Domain.Transports.Errors;

public class NotFoundTransportError : Error
{
    public NotFoundTransportError() 
        : base("Not found transport in database")
    {
        Metadata.Add("ErrorCode", 404);
    }
}