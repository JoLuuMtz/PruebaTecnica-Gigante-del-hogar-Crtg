using BookingSystem.Application.DTOs.Auth;
using FluentValidation;

namespace BookingSystem.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido.")
            .MaximumLength(100).WithMessage("El nombre de usuario no puede superar 100 caracteres.")
            .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres.")
            .Matches(@"^[a-zA-Z0-9_.-]+$").WithMessage("El nombre de usuario solo puede contener letras, números, guiones, puntos y guiones bajos.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
            .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
            .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
            .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un número.");

        RuleFor(x => x.RazonSocial)
            .NotEmpty().WithMessage("La razón social es requerida.")
            .MaximumLength(200).WithMessage("La razón social no puede superar 200 caracteres.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("El rol es requerido.")
            .Must(x => x.ToLower() == "solicitante" || x.ToLower() == "prestador" || x.ToLower() == "ambos")
            .WithMessage("El rol debe ser 'solicitante', 'prestador' o 'ambos'.");

        RuleFor(x => x.Especialidad)
            .NotEmpty().WithMessage("La especialidad es obligatoria para un prestador.")
            .MaximumLength(150).WithMessage("La especialidad no puede superar 150 caracteres.")
            .When(x => x.Role.ToLower() == "prestador" || x.Role.ToLower() == "ambos");
    }
}
