using MediatR;
using MassTransit;
using Hangfire; // Para agendar el job
using SeatingMS.Application.Commands;
using SeatingMS.Core.Repositories;
using SeatingMS.Core.DataBase;
using SeatingMS.Commons.Enums;
using SeatingMS.Commons.Events;

namespace SeatingMS.Application.Handlers.Commands
{
    public class LockSeatCommandHandler : IRequestHandler<LockSeatCommand, bool>
    {
        private readonly IEventSeatRepository _seatRepository;
        private readonly ISeatingDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IBackgroundJobClient _jobClient; // Cliente de Hangfire

        private const int LockDurationMinutes = 10;

        public LockSeatCommandHandler(
            IEventSeatRepository seatRepository,
            ISeatingDbContext context,
            IPublishEndpoint publishEndpoint,
            IBackgroundJobClient jobClient)
        {
            _seatRepository = seatRepository;
            _context = context;
            _publishEndpoint = publishEndpoint;
            _jobClient = jobClient;
        }

        public async Task<bool> Handle(LockSeatCommand request, CancellationToken cancellationToken)
        {
            var seat = await _seatRepository.GetByIdAsync(request.LockRequest.EventSeatId);

            // Validación de negocio (¡evitar race conditions!)
            if (seat == null || seat.EventId != request.LockRequest.EventId)
                throw new Exception("Asiento no encontrado.");

            if (seat.Status != SeatStatus.Available)
                throw new Exception("Asiento no disponible.");

            var lockExpiration = DateTime.UtcNow.AddMinutes(LockDurationMinutes);

            // 1. Actualizar el estado del asiento
            seat.Status = SeatStatus.Locked;
            seat.LockedByUserId = request.UserId;
            seat.LockExpiresAt = lockExpiration;

            // Iniciar transacción (para Outbox)
            await using var transaction = _context.BeginTransaction();
            try
            {
                // 2. Guardar el asiento en la BD
                await _seatRepository.UpdateAsync(seat);

                // 3. Crear evento de integración para BookingMS
                var integrationEvent = new SeatLockedEvent
                {
                    EventSeatId = seat.Id,
                    EventId = seat.EventId,
                    UserId = request.UserId,
                    Price = seat.Price,
                    LockExpiresAt = lockExpiration
                };

                // 4. Publicar al Outbox
                await _publishEndpoint.Publish(integrationEvent, cancellationToken);
                
                // 5. Guardar cambios (asiento + outbox)
                await _context.SaveChangesAsync(cancellationToken);

                // 6. Agendar el Job de Hangfire (fuera de la transacción de BD)
                _jobClient.Schedule<ISeatExpirationJob>(
                    job => job.CheckSeatLock(seat.Id),
                    TimeSpan.FromMinutes(LockDurationMinutes));

                // 7. Commit
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}