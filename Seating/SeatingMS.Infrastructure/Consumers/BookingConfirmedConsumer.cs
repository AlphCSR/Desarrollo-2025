using MassTransit;
using SeatingMS.Core.Repositories;
using SeatingMS.Commons.Enums;

namespace SeatingMS.Infrastructure.Consumers
{
    // Este evento vendrá de BookingMS
    public record BookingConfirmedIntegrationEvent
    {
        public Guid EventSeatId { get; init; }
    }

    public class BookingConfirmedConsumer : IConsumer<BookingConfirmedIntegrationEvent>
    {
        private readonly IEventSeatRepository _seatRepository;
        
        public BookingConfirmedConsumer(IEventSeatRepository seatRepository) 
            { _seatRepository = seatRepository; }

        public async Task Consume(ConsumeContext<BookingConfirmedIntegrationEvent> context)
        {
            var seat = await _seatRepository.GetByIdAsync(context.Message.EventSeatId);
            if (seat != null && seat.Status == SeatStatus.Locked)
            {
                seat.Status = SeatStatus.Sold; // ¡Vendido!
                seat.LockExpiresAt = null;
                seat.LockedByUserId = null;
                await _seatRepository.UpdateAsync(seat);
            }
        }
    }
}