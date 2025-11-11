using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Commands;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using System;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<DeletePaymentMethodCommandHandler> _logger;

        public DeletePaymentMethodCommandHandler(IPaymentGateway paymentGateway, ILogger<DeletePaymentMethodCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para eliminar un método de pago de un cliente.
        /// </summary>
        /// <param name="request">El comando DeletePaymentMethodCommand.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>True si el método de pago fue eliminado exitosamente, de lo contrario, false.</returns>
        public async Task<bool> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("DeletePaymentMethodCommandHandler: Iniciando la eliminación del método de pago {PaymentMethodId} para el cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                var result = await _paymentGateway.DetachPaymentMethodAsync(request.CustomerId, request.PaymentMethodId);
                if (result)
                {
                    _logger.LogInformation("DeletePaymentMethodCommandHandler: Método de pago {PaymentMethodId} eliminado exitosamente para el cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                }
                else
                {
                    _logger.LogWarning("DeletePaymentMethodCommandHandler: No se pudo eliminar el método de pago {PaymentMethodId} para el cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeletePaymentMethodCommandHandler: Fallo al eliminar el método de pago {PaymentMethodId} para el cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                throw;
            }
        }
    }
}
