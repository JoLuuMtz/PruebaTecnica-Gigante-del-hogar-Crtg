namespace BookingSystem.Application.DTOs.Appointments;

public class CreateAppointmentRequest
{
    public string Descripcion { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int CuposTotales { get; set; }
}
