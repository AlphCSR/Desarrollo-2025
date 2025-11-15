using MassTransit;
using BookingMS.Core.Repositories;
using BookingMS.Core.DataBase;
using BookingMS.Commons.Enums;
using BookingMS.Commons.IntegrationEvents;

namespace BookingMS.Infrastructure.Consumers
{


    public class SeatReleasedConsumer : IConsumer<SeatReleasedIntegrationEvent>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        
        public SeatReleasedConsumer(IBookingRepository bookingRepository, IBookingDbContext context, IPublishEndpoint publishEndpoint)
        {
            _bookingRepository = bookingRepository;
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<SeatReleasedIntegrationEvent> context)
        {
            var message = context.Message;
            
            // Buscar la reserva pendiente para ESE asiento
            var booking = await _bookingRepository.GetPendingBookingForSeatAsync(message.EventSeatId);

            if (booking != null && booking.Status == BookingStatus.Pending)
            {
                // El asiento se liberó, la reserva expira.
                booking.Status = BookingStatus.Expired;
                await using var transaction = _context.BeginTransaction();
                try
                {
                    await _bookingRepository.UpdateAsync(booking);

                    // Publicar que la reserva expiró/se canceló
                    await _publishEndpoint.Publish(new BookingCancelledIntegrationEvent
                    {
                        BookingId = booking.Id,
                        EventSeatId = booking.EventSeatId,
                        Reason = message.Reason // "Expired"
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