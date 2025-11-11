using MediatR;
using PaymentsMS.Core.Service;
using Microsoft.Extensions.Logging;
using PaymentsMS.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

using System;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class SetDefaultPaymentMethodCommandHandler : IRequestHandler<SetDefaultPaymentMethodCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<SetDefaultPaymentMethodCommandHandler> _logger;

        public SetDefaultPaymentMethodCommandHandler(IPaymentGateway paymentGateway, ILogger<SetDefaultPaymentMethodCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para establecer un método de pago como predeterminado para un cliente.
        /// </summary>
        /// <param name="request">El comando SetDefaultPaymentMethodCommand.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>True si el método de pago fue establecido como predeterminado exitosamente, de lo contrario, false.</returns>
        public async Task<bool> Handle(SetDefaultPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("SetDefaultPaymentMethodCommandHandler: Iniciando el establecimiento del método de pago {PaymentMethodId} como predeterminado para el cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                var result = await _paymentGateway.SetDefaultPaymentMethodAsync(request.CustomerId, request.PaymentMethodId);
                if (result)
                {
                    _logger.LogInformation("SetDefaultPaymentMethodCommandHandler: Método de pago {PaymentMethodId} establecido exitosamente como predeterminado para el cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                }
                else
                {
                    _logger.LogWarning("SetDefaultPaymentMethodCommandHandler: No se pudo establecer el método de pago {PaymentMethodId} como predeterminado para el cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SetDefaultPaymentMethodCommandHandler: Fallo al establecer el método de pago {PaymentMethodId} como predeterminado para el cliente {CustomerId}.", request.PaymentMethodId, request.CustomerId);
                throw;
            }
        }
    }
}
