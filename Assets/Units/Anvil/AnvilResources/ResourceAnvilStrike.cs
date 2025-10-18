using System.Collections.Generic;
using Units.Resources;
using UnityEngine;

namespace Units.Anvil.AnvilResources
{
    public class ResourceAnvilStrike : IUnitResource
    {
        
        private int _strikeCount;
        
        private readonly IEventBus _bus;

        public ResourceAnvilStrike(IEventBus bus)
        {
            _strikeCount = 0;
            _bus = bus; 
        }

        public ResourceId GetId()
        {
            return ResourceId.AnvilStrike;
        }
        
        public int GetValue()
        {
            return _strikeCount;
        }

        public void Increment()
        {
            int oldValue = _strikeCount;
            _strikeCount++;
            PublishChange(ResourceId.AnvilStrike, oldValue, _strikeCount);
        }
        
        private void PublishChange(ResourceId id, int oldV, int newV)
        {
            Debug.Log("Publishing new change, new value: " + newV);
            _bus.Publish(new ResourceChanged(id, oldV, newV));
        }
    }
}