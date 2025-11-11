using System;
using NotificationsMS.Domain.Entities;

namespace NotificationsMS.Domain.Exceptions
{
    public class InvalidNotificationStateTransitionException : Exception
    {
        public NotificationState CurrentState { get; }
        public NotificationState NewState { get; }

        public InvalidNotificationStateTransitionException(NotificationState currentState, NotificationState newState)
            : base($"Transición de estado no válida: no se puede cambiar de {currentState} a {newState}")
        {
            CurrentState = currentState;
            NewState = newState;
        }
    }
}
