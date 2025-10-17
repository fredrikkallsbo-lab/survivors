using Units;
using Units.Abilities;
using Units.Player;
using UnityEngine;

namespace Battlefield.GameMechanics.Combat.BattlefieldController
{
    public class BattlefieldController: MonoBehaviour
    {
        [SerializeField] private UnitTracker unitTracker;
        
        private BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;


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

    }
}