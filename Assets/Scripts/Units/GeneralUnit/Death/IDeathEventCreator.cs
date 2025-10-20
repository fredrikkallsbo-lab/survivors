using System.Collections.Generic;

namespace Units.Death
{
    public interface IDeathEventCreator
    {
        public void PublishDeathEvent(IEventBus eventBus);
    }
}