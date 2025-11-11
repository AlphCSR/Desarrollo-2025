using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PaymentsMS.Core.DTOs;
using Microsoft.Extensions.Logging;

using System;

namespace PaymentsMS.Application.Handlers.Queries
{
    public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, List<PaymentsMS.Core.DTOs.PaymentMethodDto>>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<GetPaymentMethodsQueryHandler> _logger;

        public GetPaymentMethodsQueryHandler(IPaymentGateway paymentGateway, ILogger<GetPaymentMethodsQueryHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        /// <summary>
        /// Maneja la consulta para obtener los métodos de pago de un cliente.
        /// </summary>
        /// <param name="request">La consulta GetPaymentMethodsQuery.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Una lista de PaymentMethodDto.</returns>
        public async Task<List<PaymentMethodDto>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetPaymentMethodsQueryHandler: Iniciando la obtención de métodos de pago para el cliente {CustomerId}.", request.CustomerId);
                var paymentMethods = await _paymentGateway.ListPaymentMethodsAsync(request.CustomerId);
                _logger.LogInformation("GetPaymentMethodsQueryHandler: Métodos de pago obtenidos exitosamente para el cliente {CustomerId}. Cantidad: {Count}.", request.CustomerId, paymentMethods?.Count ?? 0);
                return paymentMethods ?? new List<PaymentMethodDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaymentMethodsQueryHandler: Fallo al obtener los métodos de pago para el cliente {CustomerId}.", request.CustomerId);
                throw;
            }
        }
    }
}
