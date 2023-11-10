using FluentResults;

namespace Simbir.GO.Domain.Rents.Errors;

public class NotFoundRentError : Error
{
    public NotFoundRentError() : base("Not found rent in database")
    {
        Metadata.Add("ErrorCode", 404);
    }
}