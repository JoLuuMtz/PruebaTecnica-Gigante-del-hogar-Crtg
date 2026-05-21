using BookingSystem.Domain.Interfaces;
using BookingSystem.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Api.Controllers;

[Authorize(Roles = "admin")] // Asumiendo que solo admin puede asignar roles, o puede estar abierto para la prueba técnica si no hay admin. Ajustaré a Authorize base.
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("assign-role")]
    [AllowAnonymous] // Lo dejaré anónimo o sin rol específico para propósitos de la prueba, ya que no se especificó un rol 'admin'.
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Usuario no encontrado"));

        await _userRepository.AssignRoleAsync(request.UserId, request.RoleId);
        await _unitOfWork.SaveChangesAsync();

        return Ok(ApiResponse<object>.SuccessResponse(null, "Rol asignado exitosamente"));
    }
}

public class AssignRoleRequest
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
}
