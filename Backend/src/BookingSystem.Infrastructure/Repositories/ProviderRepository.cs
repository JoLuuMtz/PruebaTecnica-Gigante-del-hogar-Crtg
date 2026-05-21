using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using BookingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories;

public class ProviderRepository : IProviderRepository
{
    private readonly AppDbContext _context;

    public ProviderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Prestador?> GetByIdAsync(int id)
    {
        return await _context.Prestadores
            .Include(p => p.Suscriptores)
            .FirstOrDefaultAsync(p => p.CodUsuario == id);
    }

    public async Task AddAsync(Prestador prestador)
    {
        await _context.Prestadores.AddAsync(prestador);
    }

    public async Task AddSubscriptionAsync(SolicitantePrestador subscription)
    {
        await _context.SolicitantesPrestadores.AddAsync(subscription);
    }

    public async Task<bool> IsSubscribedAsync(int applicantId, int providerId)
    {
        return await _context.SolicitantesPrestadores
            .AnyAsync(sp => sp.CodUsuarioSolicitante == applicantId && sp.CodUsuarioPrestador == providerId);
    }
}
