namespace BookingSystem.Application.DTOs.Auth;

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // solicitante, prestador, ambos
    public string? Especialidad { get; set; } // Only required if role is prestador or ambos
}
