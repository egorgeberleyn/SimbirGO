using FluentResults;

namespace Simbir.GO.Domain.Rents.Errors;

public class NotIndicatedRentCostError : Error
{
    public NotIndicatedRentCostError() : base("The rental cost for this tariff is not indicated")
    {
        Metadata.Add("ErrorCode", 400);
    }
}