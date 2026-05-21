using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using BookingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cita?> GetByIdAsync(int id)
    {
        return await _context.Citas.FindAsync(id);
    }

    public async Task<Cita?> GetByIdForUpdateAsync(int id)
    {
        return await _context.Citas
            .FromSqlRaw("SELECT * FROM citas WHERE cod = {0} FOR UPDATE", id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Cita>> GetAllActiveAsync()
    {
        return await _context.Citas
            .Where(c => c.Activa && c.Fecha > DateTime.UtcNow)
            .OrderBy(c => c.Fecha)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cita>> GetByProviderIdAsync(int providerId)
    {
        return await _context.Citas
            .Where(c => c.CodUsuarioPrestador == providerId)
            .OrderByDescending(c => c.Fecha)
            .ToListAsync();
    }

    public async Task AddAsync(Cita cita)
    {
        await _context.Citas.AddAsync(cita);
    }

    public Task UpdateAsync(Cita cita)
    {
        _context.Citas.Update(cita);
        return Task.CompletedTask;
    }
}
