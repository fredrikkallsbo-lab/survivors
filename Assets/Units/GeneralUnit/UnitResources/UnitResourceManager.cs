using System.Collections.Generic;
using Units.Anvil;
using Units.Anvil.AnvilResources;
using UnityEngine;

namespace Units.Resources
{
    public class UnitResourceManager
    {
        private Dictionary<ResourceId, IUnitResource> _resources;
      
        public UnitResourceManager()
        {
            _resources = new Dictionary<ResourceId, IUnitResource>();
        }

        public UnitResourceInterface GetUnitResourceInterface()
        {
            return new UnitResourceInterface(this);
        }

        public void AddResource(ResourceId resourceId, IUnitResource unitResource)
        {
            _resources.Add(resourceId, unitResource);
        }

        public IUnitResource GetUnitResource(ResourceId resourceId)
        {
            return _resources[resourceId];
        }
    }
}