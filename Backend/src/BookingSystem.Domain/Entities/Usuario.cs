namespace BookingSystem.Domain.Entities;

public class Usuario
{
    public int Cod { get; set; }
    public string Username { get; set; } = string.Empty; // Maps to 'usuario' column
    public string Clave { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
    
    public ICollection<UsuarioRol> UsuariosRoles { get; set; } = new List<UsuarioRol>();
    public Prestador? Prestador { get; set; }
    public Solicitante? Solicitante { get; set; }
}
