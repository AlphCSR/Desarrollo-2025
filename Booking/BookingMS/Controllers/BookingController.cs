using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookingMS.Application.Queries;
using System.Security.Claims;

namespace BookingMS.Controllers
{
    [ApiController]
    [Route("booking")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IMediator mediator, ILogger<BookingController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("my-bookings")]
        //[Authorize(Roles = "Usuario, Administrador")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            _logger.LogInformation("Usuario {UserId} consultando sus reservas.", userId);

            var query = new GetUserBookingsQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        // implementar el Comando de Cancelar Reserva)
        // [HttpPost("{bookingId}/cancel")]
        // public async Task<IActionResult> CancelBooking(Guid bookingId) { ... }
    }
}