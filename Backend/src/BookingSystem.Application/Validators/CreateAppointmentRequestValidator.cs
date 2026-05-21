using BookingSystem.Application.DTOs.Appointments;
using FluentValidation;

namespace BookingSystem.Application.Validators;

public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Fecha)
            .GreaterThan(DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("Las citas deben ser para una fecha futura y no el mismo día.");
        RuleFor(x => x.CuposTotales)
            .GreaterThan(0)
            .WithMessage("Los cupos totales deben ser mayores a 0.");
    }
}
