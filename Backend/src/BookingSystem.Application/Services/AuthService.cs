using BookingSystem.Application.DTOs.Auth;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using BookingSystem.Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new BusinessException("El nombre de usuario no puede estar vacío.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new BusinessException("La contraseña no puede estar vacía.");

        if (string.IsNullOrWhiteSpace(request.RazonSocial))
            throw new BusinessException("La razón social no puede estar vacía.");

        if (string.IsNullOrWhiteSpace(request.Role))
            throw new BusinessException("El rol no puede estar vacío.");

        if (await _userRepository.ExistsByUsernameAsync(request.Username))
            throw new BusinessException("El nombre de usuario ya está en uso.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var usuario = new Usuario
        {
            Username = request.Username,
            Clave = passwordHash,
            RazonSocial = request.RazonSocial,
            Activo = true
        };

        // Asignación de Roles basándonos en la solicitud
        var rolesToAssign = new List<int>();
        var normalizedRole = request.Role.ToLower();

        if (normalizedRole == "solicitante" || normalizedRole == "ambos")
        {
            usuario.Solicitante = new Solicitante();
            rolesToAssign.Add(1); // solicitante role ID
        }

        if (normalizedRole == "prestador" || normalizedRole == "ambos")
        {
            if (string.IsNullOrWhiteSpace(request.Especialidad))
                throw new BusinessException("La especialidad es obligatoria para un prestador.");

            usuario.Prestador = new Prestador { Especialidad = request.Especialidad };
            rolesToAssign.Add(2); // prestador role ID
        }

        try
        {
            await _userRepository.AddAsync(usuario);
            await _unitOfWork.SaveChangesAsync(); // Para obtener el Id (Cod)

            // Assign roles
            foreach (var roleId in rolesToAssign)
            {
                await _userRepository.AssignRoleAsync(usuario.Cod, roleId);
            }
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new BusinessException($"Error al registrar el usuario: {ex.Message}");
        }

        // Auto-login after registration
        return await LoginAsync(new LoginRequest { Username = request.Username, Password = request.Password });
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null || !user.Activo)
            throw new BusinessException("Credenciales inválidas.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Clave))
            throw new BusinessException("Credenciales inválidas.");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ?? "M1S3cr3t0MuyS3gur0Yl4rg0P4raJWT1234567890!");

        // Ensure roles are loaded
        var roles = user.UsuariosRoles?.Select(ur => ur.Rol?.Descripcion).Where(r => r != null).ToList() ?? new List<string?>();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Cod.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        foreach (var rol in roles)
        {
            if (!string.IsNullOrEmpty(rol))
                claims.Add(new Claim(ClaimTypes.Role, rol));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponse
        {
            Token = tokenHandler.WriteToken(token),
            UserId = user.Cod,
            Username = user.Username,
            Roles = roles.Where(r => r != null).Cast<string>().ToList()
        };
    }
}
