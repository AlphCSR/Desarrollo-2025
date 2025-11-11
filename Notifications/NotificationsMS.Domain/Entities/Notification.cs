using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Domain.Entities;
using NotificationsMS.Domain.Exceptions;
using NotificationsMS.Domain.ValueObjects;

namespace NotificationsMS.Domain.Entities
{
    public enum NotificationState
    {
        Pending,
        Sent,
        Rejected        
    }

    public class Notification : Base
    {
        public Guid IdNotification { get; set; }
        public Guid IdUser { get; set; }
        public string Message { get; set; }
        public NotificationState State { get; set; }
        public DateTime CreatedAt { get; set; }

        public Notification(Guid idNotification, Guid idUser, string message, NotificationState state, DateTime createdAt)
        {
            IdNotification = idNotification;
            IdUser = idUser;
            Message = message;
            State = state;
            CreatedAt = createdAt;
        }

        public void ChangeState(NotificationState newState)
        {
            if (State == newState)
                return;

            if (!NotificationStateTransitions.IsValidTransition(State, newState))
            {
                throw new InvalidNotificationStateTransitionException(State, newState);
            }

            State = newState;
        }

        public bool CanTransitionTo(NotificationState newState)
        {
            return NotificationStateTransitions.IsValidTransition(State, newState);
        }

        public bool IsImmutable()
        {
            return NotificationStateTransitions.IsImmutable(State);
        }
    }
}
