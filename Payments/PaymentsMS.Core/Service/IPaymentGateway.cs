using System.Threading.Tasks;
using System.Collections.Generic;
using PaymentsMS.Core.DTOs;

namespace PaymentsMS.Core.Service
{
    public interface IPaymentGateway
    {
        /// <summary>
        /// Crea un nuevo cliente en el sistema de pago.
        /// </summary>
        /// <param name="email">El correo electrónico del cliente.</param>
        /// <param name="name">El nombre del cliente.</param>
        /// <returns>El ID del cliente creado.</returns>
        Task<string> CreateCustomer(string email, string name);
        /// <summary>
        /// Adjunta un método de pago a un cliente existente.
        /// </summary>
        /// <param name="customerId">El ID del cliente.</param>
        /// <param name="paymentMethodId">El ID del método de pago a adjuntar.</param>
        /// <returns>El ID del método de pago adjuntado.</returns>
        Task<string> AttachPaymentMethod(string customerId, string paymentMethodId);
        /// <summary>
        /// Desvincula un método de pago de un cliente.
        /// </summary>
        /// <param name="customerId">El ID del cliente.</param>
        /// <param name="paymentMethodId">El ID del método de pago a desvincular.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario, false.</returns>
        Task<bool> DetachPaymentMethodAsync(string customerId, string paymentMethodId);
        /// <summary>
        /// Lista los métodos de pago asociados a un cliente.
        /// </summary>
        /// <param name="customerId">El ID del cliente.</param>
        /// <returns>Una lista de objetos PaymentMethodDto.</returns>
        Task<List<PaymentMethodDto>> ListPaymentMethodsAsync(string customerId);
        /// <summary>
        /// Establece un método de pago como predeterminado para un cliente.
        /// </summary>
        /// <param name="customerId">El ID del cliente.</param>
        /// <param name="paymentMethodId">El ID del método de pago a establecer como predeterminado.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario, false.</returns>
        Task<bool> SetDefaultPaymentMethodAsync(string customerId, string paymentMethodId);
        /// <summary>
        /// Procesa un pago.
        /// </summary>
        /// <param name="customerId">El ID del cliente que realiza el pago.</param>
        /// <param name="paymentMethodId">El ID del método de pago a utilizar.</param>
        /// <param name="amount">El monto del pago.</param>
        /// <param name="currency">La moneda del pago (ej. "usd").</param>
        /// <returns>True si el pago fue procesado exitosamente, de lo contrario, false.</returns>
        Task<bool> ProcessPayment(string customerId, string paymentMethodId, long amount, string currency);
    }
}
