using BookingSystem.Domain.Entities;

namespace BookingSystem.Domain.Interfaces;

public interface IUserRepository
{
    Task<Usuario?> GetByUsernameAsync(string username);
    Task<Usuario?> GetByIdAsync(int id);
    Task AddAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
    Task<bool> ExistsByUsernameAsync(string username);
    Task AssignRoleAsync(int userId, int roleId);
}
