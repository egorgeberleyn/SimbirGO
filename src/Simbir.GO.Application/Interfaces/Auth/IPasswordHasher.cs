namespace Simbir.GO.Application.Interfaces.Auth;

public interface IPasswordHasher
{
    public (byte[] Hash, byte[] Salt) HashPassword(string password);
    public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
}