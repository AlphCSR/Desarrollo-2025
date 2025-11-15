using MediatR;
using BookingMS.Application.Queries;
using BookingMS.Commons.Dtos.Response;
using BookingMS.Core.Repositories;

namespace BookingMS.Application.Handlers.Queries
{
    public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, List<BookingDto>>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetUserBookingsQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<BookingDto>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetBookingsForUserAsync(request.UserId);

            // Mapear a DTOs
            return bookings.Select(b => new BookingDto
            {
                BookingId = b.Id,
                EventId = b.EventId,
                EventSeatId = b.EventSeatId,
                UserId = b.UserId,
                Price = b.Price,
                ExpiresAt = b.ExpiresAt,
                Status = b.Status
            }).ToList();
        }
    }
}