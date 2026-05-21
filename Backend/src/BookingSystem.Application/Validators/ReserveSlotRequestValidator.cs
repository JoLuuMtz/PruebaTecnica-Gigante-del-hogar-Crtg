using BookingSystem.Application.DTOs.Slots;
using FluentValidation;

namespace BookingSystem.Application.Validators;

public class ReserveSlotRequestValidator : AbstractValidator<ReserveSlotRequest>
{
    public ReserveSlotRequestValidator()
    {
        RuleFor(x => x.AppointmentId)
            .GreaterThan(0)
            .WithMessage("ID de cita inválido.");
    }
}
