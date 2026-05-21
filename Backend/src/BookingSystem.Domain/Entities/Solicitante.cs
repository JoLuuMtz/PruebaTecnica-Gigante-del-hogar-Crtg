namespace BookingSystem.Domain.Entities;

public class Solicitante
{
    public int CodUsuario { get; set; }
    
    public Usuario Usuario { get; set; } = null!;
    public ICollection<SolicitantePrestador> Suscripciones { get; set; } = new List<SolicitantePrestador>();
    public ICollection<Cupo> Cupos { get; set; } = new List<Cupo>();
}
