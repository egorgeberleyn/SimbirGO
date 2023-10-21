namespace Simbir.GO.Shared.Entities;

public abstract class Entity : IEquatable<Entity>
{
    public long Id {get;}
    
    public bool Equals(Entity? other) =>
        Equals((object?)other);
    
    public override bool Equals(object? obj) =>
        obj is Entity entity && Id.Equals(entity.Id);
    
    public override int GetHashCode() =>
        Id.GetHashCode();
    
    public static bool operator ==(Entity left, Entity right) =>
        left.Equals(right);
    
    public static bool operator !=(Entity left, Entity right) =>
        !left.Equals(right);
}