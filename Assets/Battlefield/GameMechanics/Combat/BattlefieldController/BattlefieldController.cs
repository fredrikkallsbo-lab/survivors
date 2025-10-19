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
        private Wanderer Wanderer { get; set; }
        private RewardFunnel _rewardFunnel;

    

        private void Awake()
        {
            Wanderer = new Wanderer();
            _rewardFunnel = new RewardFunnel(Wanderer);
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