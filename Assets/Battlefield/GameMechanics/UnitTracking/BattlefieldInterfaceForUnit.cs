using System;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units;
using Units.Abilities;

namespace Battlefield
{
    public class BattlefieldInterfaceForUnit
    {
        private BattlefieldController _battlefieldController;
       
        
        public BattlefieldInterfaceForUnit(BattlefieldController battlefieldController)
        {
            _battlefieldController = battlefieldController;
        }

        public void RegisterSpawn(Unit unit)
        {
            _battlefieldController.RegisterUnit(unit);
        }
        
        public void RegisterDeath(Unit unit)
        {
            _battlefieldController.UnregisterUnit(unit);
        }

        public void RegisterProjectile(TargetProjectile sendTargetProjectile)
        {
            _battlefieldController.RegisterProjectile(sendTargetProjectile);
        }

        public void UnregisterProjectile(TargetProjectile targetProjectile)
        {
            _battlefieldController.UnregisterProjectile(targetProjectile);
        }
    }
}