using BookingSystem.Domain.Entities;

namespace BookingSystem.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.Roles.Any())
        {
            var roles = new List<Rol>
            {
                new Rol { Cod = 1, Descripcion = "solicitante" },
                new Rol { Cod = 2, Descripcion = "prestador" }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}
