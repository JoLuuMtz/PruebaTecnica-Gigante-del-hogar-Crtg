namespace BookingSystem.Domain.Entities;

public class Rol
{
    public int Cod { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    
    public ICollection<UsuarioRol> UsuariosRoles { get; set; } = new List<UsuarioRol>();
}
