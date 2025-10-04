using Units;
using Units.Abilities;
using Units.Player;
using UnityEngine;

namespace Battlefield.GameMechanics.Combat.BattlefieldController
{
    public class BattlefieldController: MonoBehaviour
    {
        [SerializeField] private UnitTracker unitTracker;
        [SerializeField] private PlayerUnit playerUnit;
        //private ProjectileTracker _projectileTracker;
        
        private BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;


        public void RegisterUnit(Unit unit)
        {
            unitTracker.Register(unit);
        }

        public void UnregisterUnit(Unit unit)
        {
           // _projectileTracker.RegisterUnitDeath(unit);
            unitTracker.Unregister(unit);
            playerUnit.RefreshAbilityModifierSet();
            unitTracker.CheckWinOrLose();
        }


        public BattlefieldInterfaceForUnit GetBattlefieldKnowledge()
        {
           return new BattlefieldInterfaceForUnit(this);
        }

        public void RegisterProjectile(TargetProjectile targetProjectile)
        {
            //_projectileTracker.RegisterProjectile(targetProjectile);
        }

        public void UnregisterProjectile(TargetProjectile targetProjectile)
        {
            //_projectileTracker.UnregisterProjectile(targetProjectile);
        }
    }
}