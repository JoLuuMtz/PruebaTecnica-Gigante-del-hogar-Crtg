using BookingSystem.Application.DTOs.Appointments;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using BookingSystem.Shared.Exceptions;

namespace BookingSystem.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(IAppointmentRepository appointmentRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Cita> CreateAppointmentAsync(CreateAppointmentRequest request, int providerId)
    {
        var provider = await _userRepository.GetByIdAsync(providerId);
        if (provider == null || !provider.UsuariosRoles.Any(ur => ur.Rol.Descripcion.ToLower() == "prestador"))
            throw new BusinessException("El usuario no es un prestador válido.");

        if (request.Fecha.Date <= DateTime.UtcNow.Date)
            throw new BusinessException("Las citas deben ser en una fecha futura (no el mismo día).");

        if (request.CuposTotales <= 0)
            throw new BusinessException("Los cupos totales deben ser mayores a 0.");

        var cita = new Cita
        {
            Descripcion = request.Descripcion,
            Fecha = request.Fecha,
            CuposTotales = request.CuposTotales,
            CuposDisponibles = request.CuposTotales,
            CodUsuarioPrestador = providerId,
            Activa = true
        };

        await _appointmentRepository.AddAsync(cita);
        await _unitOfWork.SaveChangesAsync();

        return cita;
    }

    public async Task<IEnumerable<Cita>> GetActiveAppointmentsAsync()
    {
        return await _appointmentRepository.GetAllActiveAsync();
    }

    public async Task<IEnumerable<Cita>> GetProviderAppointmentsAsync(int providerId)
    {
        return await _appointmentRepository.GetByProviderIdAsync(providerId);
    }
}
