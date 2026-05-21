namespace BookingSystem.Domain.Entities;

public class UsuarioRol
{
    public int CodUsuario { get; set; }
    public int CodRol { get; set; }
    public DateTime FechaAsignacion { get; set; }
    
    public Usuario Usuario { get; set; } = null!;
    public Rol Rol { get; set; } = null!;
}
