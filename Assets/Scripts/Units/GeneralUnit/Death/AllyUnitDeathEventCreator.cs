using System.Collections.Generic;
using Battlefield.GameEvents;

namespace Units.Death
{
    public class AllyUnitDeathEventCreator : IDeathEventCreator
    {
        public void PublishDeathEvent(IEventBus eventBus, Unit unit)
        {
            eventBus.Publish(new AllyUnitDeathEvent());
        }
    }
}