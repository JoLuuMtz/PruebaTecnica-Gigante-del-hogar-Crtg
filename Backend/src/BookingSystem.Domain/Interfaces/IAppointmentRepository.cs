using BookingSystem.Domain.Entities;

namespace BookingSystem.Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<Cita?> GetByIdAsync(int id);
    Task<Cita?> GetByIdForUpdateAsync(int id);
    Task<IEnumerable<Cita>> GetAllActiveAsync();
    Task<IEnumerable<Cita>> GetByProviderIdAsync(int providerId);
    Task AddAsync(Cita cita);
    Task UpdateAsync(Cita cita);
}
