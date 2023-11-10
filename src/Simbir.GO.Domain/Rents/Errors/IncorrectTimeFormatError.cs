using FluentResults;

namespace Simbir.GO.Domain.Rents.Errors;

public class IncorrectTimeFormatError : Error
{
    public IncorrectTimeFormatError() : base("Incorrect time format")
    {
        Metadata.Add("ErrorCode", 400);
    }
}