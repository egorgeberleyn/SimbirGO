using Simbir.GO.Application.Interfaces;

namespace Simbir.GO.Infrastructure.Utils;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}