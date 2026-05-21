namespace BookingSystem.Domain.Entities;

public class Cupo
{
    public int CodCita { get; set; }
    public int CodUsuarioSolicitante { get; set; }
    public DateTime FechaReserva { get; set; }
    
    public Cita Cita { get; set; } = null!;
    public Solicitante Solicitante { get; set; } = null!;
}
