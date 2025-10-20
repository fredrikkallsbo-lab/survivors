using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.loot;
using Units;
using Units.Abilities;
using UnityEngine;

namespace Battlefield.GameMechanics.Combat.BattlefieldController
{
    public class BattlefieldController: MonoBehaviour
    {
        [SerializeField] private UnitTracker unitTracker;
        
        private BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;
        private readonly IEventBus  _eventBus = new EventBus();
        private RewardFunnel _rewardFunnel;


        public void RegisterWanderer(Wanderer wanderer)
        {
            _rewardFunnel = new RewardFunnel(wanderer);
        }

        public void RegisterUnit(Unit unit)
        {
            unitTracker.Register(unit);
        }

        public void UnregisterUnit(Unit unit)
        {
            unitTracker.Unregister(unit);
            _rewardFunnel.AddExperience(1);
            Destroy(unit.gameObject);
        }


        public BattlefieldInterfaceForUnit GetBattlefieldUnitInterface()
        {
           return new BattlefieldInterfaceForUnit(this);
        }

        public IEventBus GetEventBus()
        {
            return _eventBus;
        }

    }
}