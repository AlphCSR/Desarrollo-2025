using MediatR;
using BookingMS.Commons.Dtos.Response;

namespace BookingMS.Application.Queries
{
    public class GetUserBookingsQuery : IRequest<List<BookingDto>>
    {
        public string UserId { get; }
        
        public GetUserBookingsQuery(string userId)
        {
            UserId = userId;
        }
    }
}