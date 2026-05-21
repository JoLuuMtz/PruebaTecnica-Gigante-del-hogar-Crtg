using BookingSystem.Application.DTOs.Slots;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using BookingSystem.Shared.Exceptions;

namespace BookingSystem.Application.Services;

public class SlotService : ISlotService
{
    private readonly ISlotRepository _slotRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SlotService(ISlotRepository slotRepository, IAppointmentRepository appointmentRepository, IProviderRepository providerRepository, IUnitOfWork unitOfWork)
    {
        _slotRepository = slotRepository;
        _appointmentRepository = appointmentRepository;
        _providerRepository = providerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ReserveSlotAsync(ReserveSlotRequest request, int applicantId)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Obtener cita con Row Locking (FOR UPDATE) usando el repositorio
            var cita = await _appointmentRepository.GetByIdForUpdateAsync(request.AppointmentId);

            if (cita == null || !cita.Activa)
                throw new NotFoundException("Cita no encontrada o no está activa.");

            if (cita.Fecha < DateTime.UtcNow)
                throw new BusinessException("No se pueden reservar citas pasadas.");

            if (cita.CuposDisponibles <= 0)
                throw new BusinessException("No hay cupos disponibles.");

            // Validar suscripción
            var isSubscribed = await _providerRepository.IsSubscribedAsync(applicantId, cita.CodUsuarioPrestador);
            if (!isSubscribed)
                throw new BusinessException("Debe estar suscrito al prestador para reservar una cita.");

            // Validar no doble reserva
            if (await _slotRepository.HasReservedSlotAsync(request.AppointmentId, applicantId))
                throw new BusinessException("Ya tiene un cupo reservado en esta cita.");

            // Disminuir cupos usando EF Core. 
            // En un escenario real con MySQL y Row Locking, esto se complementaría con un raw SQL query for update,
            // pero lo implementaremos usando el UnitOfWork con IsolationLevel que hemos definido en el controller/service.
            
            // Efectuar reserva
            var cupo = new Cupo
            {
                CodCita = request.AppointmentId,
                CodUsuarioSolicitante = applicantId,
            };

            await _slotRepository.AddReservationAsync(cupo);
            
            cita.CuposDisponibles -= 1;
            await _appointmentRepository.UpdateAsync(cita);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
