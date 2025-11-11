
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentsMS.Application.Commands;
using System.Threading.Tasks;
using PaymentsMS.Core.DTOs;
using PaymentsMS.Application.Queries;
using System;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Filters;
using PaymentsMS.Examples.Commands;

namespace PaymentsMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IMediator mediator, ILogger<PaymentsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

                [HttpPost("customer")]
        [SwaggerRequestExample(typeof(CreateCustomerCommand), typeof(CreateCustomerCommandExample))]
        /// <summary>
        /// Crea un nuevo cliente en el sistema de pagos.
        /// </summary>
        /// <param name="command">Comando para crear un cliente, incluyendo los detalles necesarios.</param>
        /// <returns>Un IActionResult que contiene el ID del cliente si la creación fue exitosa.</returns>
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            if (command == null)
            {
                _logger.LogWarning("CreateCustomer: El comando de creación de cliente recibido es nulo.");
                return BadRequest(new { message = "El comando para crear el cliente no puede ser nulo." });
            }

            _logger.LogInformation("CreateCustomer: Intentando crear un nuevo cliente.");
            var customerId = await _mediator.Send(command);
            if (customerId == null)
            {
                _logger.LogError("CreateCustomer: Fallo al crear el cliente. El ID de cliente retornado es nulo.");
                return BadRequest(new { message = "Fallo al crear el cliente. Verifique los datos proporcionados." });
            }

            _logger.LogInformation("CreateCustomer: Cliente creado exitosamente con ID: {CustomerId}.", customerId);
            return Ok(new { customerId });
        }

        [HttpPost("payment-method")]
        [SwaggerRequestExample(typeof(AttachPaymentMethodCommand), typeof(AttachPaymentMethodCommandExample))]
        /// <summary>
        /// Adjunta un nuevo método de pago a un cliente existente.
        /// </summary>
        /// <param name="command">Comando para adjuntar un método de pago, incluyendo el ID del cliente y los detalles del método de pago.</param>
        /// <returns>Un IActionResult que contiene el ID del método de pago si la operación fue exitosa.</returns>
        public async Task<IActionResult> AttachPaymentMethod([FromBody] AttachPaymentMethodCommand command)
        {
            if (command == null)
            {
                _logger.LogWarning("AttachPaymentMethod: El comando para adjuntar el método de pago recibido es nulo.");
                return BadRequest(new { message = "El comando para adjuntar el método de pago no puede ser nulo." });
            }

            _logger.LogInformation("AttachPaymentMethod: Intentando adjuntar un nuevo método de pago al cliente.");
            var paymentMethodId = await _mediator.Send(command);
            if (paymentMethodId == null)
            {
                _logger.LogError("AttachPaymentMethod: Fallo al adjuntar el método de pago. El ID del método de pago retornado es nulo.");
                return BadRequest(new { message = "Fallo al adjuntar el método de pago. Verifique los datos proporcionados." });
            }

            _logger.LogInformation("AttachPaymentMethod: Método de pago adjuntado exitosamente con ID: {PaymentMethodId}.", paymentMethodId);
            return  Ok(new { paymentMethodId });
        }

        [HttpDelete("payment-method")]
        /// <summary>
        /// Elimina un método de pago de un cliente.
        /// </summary>
        /// <param name="customerId">ID del cliente al que pertenece el método de pago.</param>
        /// <param name="paymentMethodId">ID del método de pago a eliminar.</param>
        /// <returns>Un IActionResult indicando el éxito o fracaso de la operación.</returns>
        public async Task<IActionResult> DeletePaymentMethod([FromQuery] string customerId, [FromQuery] string paymentMethodId)
        {
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(paymentMethodId))
            {
                _logger.LogWarning("DeletePaymentMethod: CustomerId o PaymentMethodId son nulos o vacíos.");
                return BadRequest(new { message = "El ID del cliente y el ID del método de pago son requeridos." });
            }

            var command = new DeletePaymentMethodCommand { CustomerId = customerId, PaymentMethodId = paymentMethodId };
            _logger.LogInformation("DeletePaymentMethod: Intentando eliminar el método de pago {PaymentMethodId} para el cliente {CustomerId}.", paymentMethodId, customerId);
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation("DeletePaymentMethod: Método de pago {PaymentMethodId} eliminado exitosamente para el cliente {CustomerId}.", paymentMethodId, customerId);
                return Ok(new { message = "Metodo de pago eliminado exitosamente." });
            }
            _logger.LogError("DeletePaymentMethod: Fallo al eliminar el método de pago {PaymentMethodId} para el cliente {CustomerId}.", paymentMethodId, customerId);
            return BadRequest(new { message = "Fallo al eliminar el metodo de pago." });
        }

        /// <summary>
        /// Obtiene todos los métodos de pago asociados a un cliente.
        /// </summary>
        /// <param name="customerId">ID del cliente para el cual se desean obtener los métodos de pago.</param>
        /// <returns>Un IActionResult que contiene una lista de PaymentMethodDto si la operación fue exitosa.</returns>
        [HttpGet("payment-methods")]
        public async Task<IActionResult> GetPaymentMethods([FromQuery] string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                _logger.LogWarning("GetPaymentMethods: CustomerId es nulo o vacío.");
                return BadRequest(new { message = "El ID del cliente es requerido para obtener los métodos de pago." });
            }

            var query = new GetPaymentMethodsQuery { CustomerId = customerId };
            _logger.LogInformation("GetPaymentMethods: Intentando obtener métodos de pago para el cliente {CustomerId}.", customerId);
            var paymentMethods = await _mediator.Send(query);
            
            if (paymentMethods == null || !paymentMethods.Any())
            {
                _logger.LogInformation("GetPaymentMethods: No se encontraron métodos de pago para el cliente {CustomerId}.", customerId);
                return NotFound(new { message = "No se encontraron métodos de pago para el cliente especificado." });
            }

            _logger.LogInformation("GetPaymentMethods: Métodos de pago obtenidos exitosamente para el cliente {CustomerId}.", customerId);
            return Ok(paymentMethods);
        }

        /// <summary>
        /// Establece un método de pago como predeterminado para un cliente.
        /// </summary>
        /// <param name="command">Comando para establecer el método de pago por defecto, incluyendo el ID del cliente y el ID del método de pago.</param>
        /// <returns>Un IActionResult indicando el éxito o fracaso de la operación.</returns>
        [HttpPatch("default-payment-method")]
        [SwaggerRequestExample(typeof(SetDefaultPaymentMethodCommand), typeof(SetDefaultPaymentMethodCommandExample))]
        public async Task<IActionResult> SetDefaultPaymentMethod([FromBody] SetDefaultPaymentMethodCommand command)
        {
            if (command == null)
            {
                _logger.LogWarning("SetDefaultPaymentMethod: El comando para establecer el método de pago por defecto recibido es nulo.");
                return BadRequest(new { message = "El comando para establecer el método de pago por defecto no puede ser nulo." });
            }

            _logger.LogInformation("SetDefaultPaymentMethod: Intentando establecer el método de pago {PaymentMethodId} como predeterminado para el cliente {CustomerId}.", command.PaymentMethodId, command.CustomerId);
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation("SetDefaultPaymentMethod: Método de pago {PaymentMethodId} establecido exitosamente como predeterminado para el cliente {CustomerId}.", command.PaymentMethodId, command.CustomerId);
                return Ok(new { message = "Metodo de pago por defecto establecido exitosamente." });
            }
            _logger.LogError("SetDefaultPaymentMethod: Fallo al establecer el método de pago {PaymentMethodId} como predeterminado para el cliente {CustomerId}.", command.PaymentMethodId, command.CustomerId);
            return BadRequest(new { message = "Fallo al establecer el metodo de pago por defecto." });
        }

        /// <summary>
        /// Procesa un pago utilizando la información proporcionada.
        /// </summary>
        /// <param name="command">Comando para procesar un pago, incluyendo los detalles de la transacción.</param>
        /// <returns>Un IActionResult indicando el éxito o fracaso de la operación.</returns>
        [HttpPost("process-payment")]
        [SwaggerRequestExample(typeof(ProcessPaymentCommand), typeof(ProcessPaymentCommandExample))]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
        {
            if (command == null)
            {
                _logger.LogWarning("ProcessPayment: El comando de procesamiento de pago recibido es nulo.");
                return BadRequest(new { message = "El comando para procesar el pago no puede ser nulo." });
            }

            _logger.LogInformation("ProcessPayment: Intentando procesar un pago para el cliente {CustomerId} con un monto de {Amount}.", command.CustomerId, command.Amount);
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation("ProcessPayment: Pago procesado exitosamente para el cliente {CustomerId}.", command.CustomerId);
                return Ok(new { message = "Pago procesado exitosamente." });
            }
            _logger.LogError("ProcessPayment: Fallo al procesar el pago para el cliente {CustomerId}.", command.CustomerId);
            return BadRequest(new { message = "Fallo al procesar el pago." });
        }
    }
}
