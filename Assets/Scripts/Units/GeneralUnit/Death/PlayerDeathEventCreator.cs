using System.Collections.Generic;
using Units.GeneralUnit.DeathManagement;

namespace Units.Death
{
    public class PlayerDeathEventCreator :  IDeathEventCreator
    {
        public void PublishDeathEvent(IEventBus eventBus)
        {
            eventBus.Publish(new PlayerDeathEvent());
        }
    }
}