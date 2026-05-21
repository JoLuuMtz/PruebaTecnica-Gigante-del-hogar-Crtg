using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using BookingSystem.Shared.Exceptions;
using BookingSystem.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProvidersController : ControllerBase
{
    private readonly IProviderRepository _providerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProvidersController(IProviderRepository providerRepository, IUnitOfWork unitOfWork)
    {
        _providerRepository = providerRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("subscribe")]
    [Authorize(Roles = "solicitante")]
    public async Task<IActionResult> Subscribe([FromBody] int providerId)
    {
        var applicantId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        if (await _providerRepository.IsSubscribedAsync(applicantId, providerId))
            throw new BusinessException("Ya está suscrito a este prestador.");

        var provider = await _providerRepository.GetByIdAsync(providerId);
        if (provider == null)
            throw new NotFoundException("Prestador no encontrado.");

        var subscription = new SolicitantePrestador
        {
            CodUsuarioSolicitante = applicantId,
            CodUsuarioPrestador = providerId
        };

        await _providerRepository.AddSubscriptionAsync(subscription);
        await _unitOfWork.SaveChangesAsync();

        return Ok(ApiResponse<object>.SuccessResponse(null, "Suscripción exitosa"));
    }
}
