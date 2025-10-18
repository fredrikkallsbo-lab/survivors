using System.Collections.Generic;
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

        public void RegisterUnit(Unit unit)
        {
            unitTracker.Register(unit);
        }

        public void UnregisterUnit(Unit unit)
        {
            unitTracker.Unregister(unit);
            unitTracker.CheckWinOrLose();
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