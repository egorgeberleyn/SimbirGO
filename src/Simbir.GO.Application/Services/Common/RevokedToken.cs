using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Application.Services.Common;

public class RevokedToken : Entity
{
    public long UserId { get; set; }
    public string Token { get; set; } = null!;
    public string JwtId { get; set; } = null!;
    public bool IsRevoked { get; set; }
    public DateTime AddedDate { get; set; }
}