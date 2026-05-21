using BookingSystem.Application.DTOs.Appointments;
using BookingSystem.Application.Interfaces;
using BookingSystem.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    [Authorize(Roles = "prestador")]
    public async Task<IActionResult> CreateAppointment(CreateAppointmentRequest request)
    {
        var providerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _appointmentService.CreateAppointmentAsync(request, providerId);
        return Ok(ApiResponse<object>.SuccessResponse(new { result.Cod, result.Descripcion, result.Fecha, result.CuposTotales }, "Cita creada exitosamente"));
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveAppointments()
    {
        var result = await _appointmentService.GetActiveAppointmentsAsync();
        var mapped = result.Select(c => new { c.Cod, c.Descripcion, c.Fecha, c.CuposTotales, c.CuposDisponibles, c.CodUsuarioPrestador });
        return Ok(ApiResponse<object>.SuccessResponse(mapped));
    }

    [HttpGet("provider/{id}")]
    public async Task<IActionResult> GetProviderAppointments(int id)
    {
        var result = await _appointmentService.GetProviderAppointmentsAsync(id);
        var mapped = result.Select(c => new { c.Cod, c.Descripcion, c.Fecha, c.CuposTotales, c.CuposDisponibles });
        return Ok(ApiResponse<object>.SuccessResponse(mapped));
    }
}
