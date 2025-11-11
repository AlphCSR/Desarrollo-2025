using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Commands;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using System;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class AttachPaymentMethodCommandHandler : IRequestHandler<AttachPaymentMethodCommand, string>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<AttachPaymentMethodCommandHandler> _logger;

        public AttachPaymentMethodCommandHandler(IPaymentGateway paymentGateway, ILogger<AttachPaymentMethodCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para adjuntar un método de pago a un cliente.
        /// </summary>
        /// <param name="request">El comando AttachPaymentMethodCommand.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>El ID del método de pago adjuntado.</returns>
        public async Task<string> Handle(AttachPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("AttachPaymentMethodCommandHandler: Iniciando el proceso para adjuntar el método de pago {PaymentMethodId} al cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                var paymentMethodId = await _paymentGateway.AttachPaymentMethod(request.CustomerId, request.PaymentMethodId);
                _logger.LogInformation("AttachPaymentMethodCommandHandler: Método de pago {PaymentMethodId} adjuntado exitosamente al cliente {CustomerId}.", paymentMethodId, request.CustomerId);
                return paymentMethodId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AttachPaymentMethodCommandHandler: Fallo al adjuntar el método de pago {PaymentMethodId} al cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                throw;
            }
        }   
    }
}
