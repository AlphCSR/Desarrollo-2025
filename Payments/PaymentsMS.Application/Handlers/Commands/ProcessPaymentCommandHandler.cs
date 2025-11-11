using MediatR;
using PaymentsMS.Application.Commands;
using PaymentsMS.Core.Service;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;


using System;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<ProcessPaymentCommandHandler> _logger;

        public ProcessPaymentCommandHandler(IPaymentGateway paymentGateway, ILogger<ProcessPaymentCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para procesar un pago.
        /// </summary>
        /// <param name="request">El comando ProcessPaymentCommand.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>True si el pago fue procesado exitosamente, de lo contrario, false.</returns>
        public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("ProcessPaymentCommandHandler: Iniciando el procesamiento de pago para el cliente {CustomerId} con monto {Amount} {Currency}.", request.CustomerId, request.Amount, request.Currency);
                var result = await _paymentGateway.ProcessPayment(request.CustomerId, request.PaymentMethodId, request.Amount, request.Currency);
                if (result)
                {
                    _logger.LogInformation("ProcessPaymentCommandHandler: Pago procesado exitosamente para el cliente {CustomerId}.", request.CustomerId);
                }
                else
                {
                    _logger.LogWarning("ProcessPaymentCommandHandler: El procesamiento de pago para el cliente {CustomerId} no fue exitoso.", request.CustomerId);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProcessPaymentCommandHandler: Fallo al procesar el pago para el cliente {CustomerId} con monto {Amount} {Currency}.", request.CustomerId, request.Amount, request.Currency);
                throw;
            }
        }
    }
}
