using BookingSystem.Application.DTOs.Slots;

namespace BookingSystem.Application.Interfaces;

public interface ISlotService
{
    Task ReserveSlotAsync(ReserveSlotRequest request, int applicantId);
}
