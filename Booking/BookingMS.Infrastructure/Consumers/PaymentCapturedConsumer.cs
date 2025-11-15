using MassTransit;
using BookingMS.Core.Repositories;
using BookingMS.Core.DataBase;
using BookingMS.Commons.Enums;
using BookingMS.Commons.IntegrationEvents;

namespace BookingMS.Infrastructure.Consumers
{
    public class PaymentCapturedConsumer : IConsumer<PaymentCapturedIntegrationEvent>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        
        public PaymentCapturedConsumer(IBookingRepository bookingRepository, IBookingDbContext context, IPublishEndpoint publishEndpoint)
        {
            _bookingRepository = bookingRepository;
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<PaymentCapturedIntegrationEvent> context)
        {
            var booking = await _bookingRepository.GetByIdAsync(context.Message.BookingId);

            if (booking != null && booking.Status == BookingStatus.Pending)
            {
                // ¡Pago confirmado!
                booking.Status = BookingStatus.Confirmed;
                
                await using var transaction = _context.BeginTransaction();
                try
                {
                    await _bookingRepository.UpdateAsync(booking);

                    // Publicar evento de Confirmación
                    await _publishEndpoint.Publish(new BookingConfirmedIntegrationEvent
                    {
                        BookingId = booking.Id,
                        EventSeatId = booking.EventSeatId,
                        UserId = booking.UserId,
                        EventId = booking.EventId
                    });
                    
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}