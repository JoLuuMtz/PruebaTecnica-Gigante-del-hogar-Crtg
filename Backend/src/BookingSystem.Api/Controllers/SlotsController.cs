using BookingSystem.Application.DTOs.Slots;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Interfaces;
using BookingSystem.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SlotsController : ControllerBase
{
    private readonly ISlotService _slotService;
    private readonly ISlotRepository _slotRepository;

    public SlotsController(ISlotService slotService, ISlotRepository slotRepository)
    {
        _slotService = slotService;
        _slotRepository = slotRepository;
    }

    [HttpPost("reserve")]
    [Authorize(Roles = "solicitante")]
    public async Task<IActionResult> ReserveSlot(ReserveSlotRequest request)
    {
        var applicantId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        await _slotService.ReserveSlotAsync(request, applicantId);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Reserva realizada exitosamente"));
    }

    [HttpGet("my-reservations")]
    [Authorize(Roles = "solicitante")]
    public async Task<IActionResult> GetMyReservations()
    {
        var applicantId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _slotRepository.GetReservationsByApplicantIdAsync(applicantId);
        var mapped = result.Select(c => new { 
            c.CodCita, 
            c.Cita.Descripcion, 
            c.Cita.Fecha, 
            Prestador = c.Cita.Prestador.Usuario.RazonSocial,
            c.FechaReserva 
        });
        return Ok(ApiResponse<object>.SuccessResponse(mapped));
    }
}
