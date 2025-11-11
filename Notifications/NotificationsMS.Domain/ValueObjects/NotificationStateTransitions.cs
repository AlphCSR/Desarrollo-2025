using System.Collections.Generic;
using NotificationsMS.Domain.Entities;

namespace NotificationsMS.Domain.ValueObjects
{
    public static class NotificationStateTransitions
    {
        private static readonly Dictionary<NotificationState, List<NotificationState>> ValidTransitions = new()
        {
            { NotificationState.Pending, new List<NotificationState> { NotificationState.Sent } },
            { NotificationState.Sent, new List<NotificationState> { NotificationState.Rejected } },
            { NotificationState.Rejected, new List<NotificationState>() },
        };

        public static bool IsValidTransition(NotificationState currentState, NotificationState newState)
        {
            // No se permite cambiar al mismo estado
            if (currentState == newState)
                return false;

            // Verificar si la transición es válida
            return ValidTransitions.ContainsKey(currentState) && 
                   ValidTransitions[currentState].Contains(newState);
        }

        public static bool IsImmutable(NotificationState state)
        {
            // Las notificaciones en rechazados no se pueden modificar
            return state == NotificationState.Rejected;
        }
    }
}
