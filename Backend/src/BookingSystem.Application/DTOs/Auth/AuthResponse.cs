namespace BookingSystem.Application.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}
