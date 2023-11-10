namespace Simbir.GO.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}