namespace Simbir.GO.Application.Services.Common;

public record RefreshToken()
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Token { get; set; } = null!;
    public string JwtId { get; set; } = null!;
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
}