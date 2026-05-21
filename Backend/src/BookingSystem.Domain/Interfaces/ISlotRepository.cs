using BookingSystem.Domain.Entities;

namespace BookingSystem.Domain.Interfaces;

public interface ISlotRepository
{
    Task<bool> HasReservedSlotAsync(int appointmentId, int applicantId);
    Task AddReservationAsync(Cupo cupo);
    Task<IEnumerable<Cupo>> GetReservationsByApplicantIdAsync(int applicantId);
}
