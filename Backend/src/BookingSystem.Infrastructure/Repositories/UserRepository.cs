using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using BookingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        return await _context.Usuarios
            .Include(u => u.UsuariosRoles)
            .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios
            .Include(u => u.UsuariosRoles)
            .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync(u => u.Cod == id);
    }

    public async Task AddAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public Task UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _context.Usuarios.AnyAsync(u => u.Username == username);
    }

    public async Task AssignRoleAsync(int userId, int roleId)
    {
        var usuarioRol = new UsuarioRol
        {
            CodUsuario = userId,
            CodRol = roleId,
            FechaAsignacion = DateTime.UtcNow
        };
        await _context.UsuariosRoles.AddAsync(usuarioRol);
    }
}
