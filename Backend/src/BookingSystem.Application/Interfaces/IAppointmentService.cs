using BookingSystem.Application.DTOs.Appointments;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IAppointmentService
{
    Task<Cita> CreateAppointmentAsync(CreateAppointmentRequest request, int providerId);
    Task<IEnumerable<Cita>> GetActiveAppointmentsAsync();
    Task<IEnumerable<Cita>> GetProviderAppointmentsAsync(int providerId);
}
