namespace BookingSystem.Domain.Entities;

public class Prestador
{
    public int CodUsuario { get; set; }
    public string? Especialidad { get; set; }
    
    public Usuario Usuario { get; set; } = null!;
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public ICollection<SolicitantePrestador> Suscriptores { get; set; } = new List<SolicitantePrestador>();
}
