namespace BookingSystem.Domain.Entities;

public class Cita
{
    public int Cod { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int CuposTotales { get; set; }
    public int CuposDisponibles { get; set; }
    public int CodUsuarioPrestador { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Activa { get; set; }
    
    public Prestador Prestador { get; set; } = null!;
    public ICollection<Cupo> Cupos { get; set; } = new List<Cupo>();
}
