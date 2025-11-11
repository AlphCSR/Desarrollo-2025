using MediatR;
using System.Collections.Generic;
using PaymentsMS.Core.DTOs;

namespace PaymentsMS.Application.Queries
{
    public class GetPaymentMethodsQuery : IRequest<List<PaymentsMS.Core.DTOs.PaymentMethodDto>>
    {
        public string CustomerId { get; set; } = string.Empty;
    }
}
