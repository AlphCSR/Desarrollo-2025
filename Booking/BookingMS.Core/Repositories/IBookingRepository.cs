using BookingMS.Domain.Entities;

namespace BookingMS.Core.Repositories
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task<Booking?> GetByIdAsync(Guid bookingId);
        Task<Booking?> GetPendingBookingForSeatAsync(Guid eventSeatId);
        Task<List<Booking>> GetBookingsForUserAsync(string userId);
    }
}