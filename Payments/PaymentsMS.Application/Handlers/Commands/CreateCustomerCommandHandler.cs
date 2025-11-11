using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Commands;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, string>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(IPaymentGateway paymentGateway, ILogger<CreateCustomerCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para crear un nuevo cliente.
        /// </summary>
        /// <param name="request">El comando CreateCustomerCommand.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>El ID del cliente creado.</returns>
        public async Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("CreateCustomerCommandHandler: Iniciando la creación de un nuevo cliente con email {Email}.", request.Email);
                var customerId = await _paymentGateway.CreateCustomer(request.Email, request.Name);
                _logger.LogInformation("CreateCustomerCommandHandler: Cliente creado exitosamente con ID: {CustomerId}.", customerId);
                return customerId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateCustomerCommandHandler: Fallo al crear el cliente con email {Email}.", request.Email);
                throw;
            }
        }
    }
}
