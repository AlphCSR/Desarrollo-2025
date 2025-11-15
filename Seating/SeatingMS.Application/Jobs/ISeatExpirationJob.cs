namespace SeatingMS.Application.Jobs
{
    public interface ISeatExpirationJob
    {
        Task CheckSeatLock(Guid eventSeatId);
    }
}