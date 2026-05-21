namespace BookingSystem.Domain.Entities;

public class SolicitantePrestador
{
    public int CodUsuarioSolicitante { get; set; }
    public int CodUsuarioPrestador { get; set; }
    public DateTime FechaSuscripcion { get; set; }
    
    public Solicitante Solicitante { get; set; } = null!;
    public Prestador Prestador { get; set; } = null!;
}
