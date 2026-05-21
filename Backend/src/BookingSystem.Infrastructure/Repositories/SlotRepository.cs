using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using BookingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories;

public class SlotRepository : ISlotRepository
{
    private readonly AppDbContext _context;

    public SlotRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasReservedSlotAsync(int appointmentId, int applicantId)
    {
        return await _context.Cupos
            .AnyAsync(c => c.CodCita == appointmentId && c.CodUsuarioSolicitante == applicantId);
    }

    public async Task AddReservationAsync(Cupo cupo)
    {
        await _context.Cupos.AddAsync(cupo);
    }

    public async Task<IEnumerable<Cupo>> GetReservationsByApplicantIdAsync(int applicantId)
    {
        return await _context.Cupos
            .Include(c => c.Cita)
            .ThenInclude(cita => cita.Prestador)
            .ThenInclude(p => p.Usuario)
            .Where(c => c.CodUsuarioSolicitante == applicantId)
            .OrderByDescending(c => c.FechaReserva)
            .ToListAsync();
    }
}
