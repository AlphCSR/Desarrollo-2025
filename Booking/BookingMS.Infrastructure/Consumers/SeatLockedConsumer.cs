using MassTransit;
using BookingMS.Core.Repositories;
using BookingMS.Core.DataBase;
using BookingMS.Domain.Entities;
using BookingMS.Commons.Enums;
using BookingMS.Commons.Events;

namespace BookingMS.Infrastructure.Consumers
{
    public class SeatLockedConsumer : IConsumer<SeatLockedEvent>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public SeatLockedConsumer(IBookingRepository bookingRepository, IBookingDbContext context, IPublishEndpoint publishEndpoint)
        {
            _bookingRepository = bookingRepository;
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<SeatLockedEvent> context)
        {
            var message = context.Message;
            
            var newBooking = new Booking
            {
                Id = Guid.NewGuid(),
                UserId = message.UserId,
                EventId = message.EventId,
                EventSeatId = message.EventSeatId,
                Price = message.Price,
                ExpiresAt = message.LockExpiresAt,
                Status = BookingStatus.Pending, // Nace como Pendiente
                CreatedAt = DateTime.UtcNow
            };

            await using var transaction = _context.BeginTransaction();
            try
            {
                await _bookingRepository.AddAsync(newBooking);

                // Publicar nuestro propio evento
                await _publishEndpoint.Publish(new BookingCreatedIntegrationEvent
                {
                    BookingId = newBooking.Id,
                    UserId = newBooking.UserId,
                    EventId = newBooking.EventId,
                    Price = newBooking.Price,
                    ExpiresAt = newBooking.ExpiresAt
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