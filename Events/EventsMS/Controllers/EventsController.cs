using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EventsMS.Application.Commands;
// using EventsMS.Application.Queries; // (Necesitarías crear estos)
using EventsMS.Commons.Dtos.Request;
using System.Security.Claims; // Para leer el ID del usuario

namespace EventsMS.Controllers
{
    [ApiController]
    [Route("events")]
    [Authorize] // Proteger todo el controlador
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IMediator _mediator;

        public EventsController(ILogger<EventsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Roles = "Organizador, Administrador")] // Solo estos roles pueden crear [cite: 196, 197]
        public async Task<IActionResult> CreateEvent(CreateEventDto createEventDto)
        {
            // Obtener el ID del usuario (Organizer) desde el token
            var organizerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(organizerId))
            {
                return Unauthorized("No se pudo identificar al organizador.");
            }

            _logger.LogInformation("Recibida solicitud para crear evento por {OrganizerId}", organizerId);
            
            try
            {
                var command = new CreateEventCommand(createEventDto, organizerId);
                var resultDto = await _mediator.Send(command);
                
                // Devolver un 201 Created con la ubicación del nuevo recurso
                return CreatedAtAction(nameof(GetEventById), new { id = resultDto.EventId }, resultDto);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creando evento.");
                return StatusCode(500, "Error interno al crear el evento.");
            }
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous] // Permitir que todos vean los eventos
        public async Task<IActionResult> GetEventById(Guid id)
        {
            return Ok(new { Message = $"Endpoint para GetEventById {id} no implementado." });
        }
        
        [HttpGet]
        [AllowAnonymous] // Permitir que todos vean los eventos
        public async Task<IActionResult> GetAllEvents()
        {
            return Ok(new { Message = "Endpoint para GetAllEvents no implementado." });
        }
    }
}