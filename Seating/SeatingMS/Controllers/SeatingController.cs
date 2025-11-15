using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SeatingMS.Application.Commands;
using SeatingMS.Application.Queries; // (Debes crear el Query)
using SeatingMS.Commons.Dtos.Request;
using System.Security.Claims;

namespace SeatingMS.Controllers
{
    [ApiController]
    [Route("seating")]
    public class SeatingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SeatingController> _logger;
        
        public SeatingController(IMediator mediator, ILogger<SeatingController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("event/{eventId}")]
        [AllowAnonymous] // Todos pueden ver el mapa
        public async Task<IActionResult> GetSeatMapForEvent(Guid eventId)
        {
            return Ok(new { Message = $"Mapa de asientos para {eventId} no implementado."});
        }

        [HttpPost("lock")]
        //[Authorize(Roles = "Usuario, Administrador")] // Solo usuarios logueados pueden bloquear
        public async Task<IActionResult> LockSeat(LockSeatRequestDto lockRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            try
            {
                var command = new LockSeatCommand(lockRequest, userId);
                var success = await _mediator.Send(command);
                if (success)
                    return Ok(new { Message = "Asiento bloqueado por 10 minutos." });
                else
                    return BadRequest("No se pudo bloquear el asiento.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al bloquear asiento {EventSeatId}", lockRequest.EventSeatId);
                return BadRequest(new { Error = e.Message });
            }
        }
    }
}