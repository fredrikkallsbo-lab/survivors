using System.Collections.Generic;

namespace Units.GeneralUnit.DeathManagement
{
    public class PlayerDeathEvent : IGameEvent
    {
        public string Reason { get; }

        public PlayerDeathEvent(string reason = "You died")
        {
            Reason = reason;
        }
    }
}