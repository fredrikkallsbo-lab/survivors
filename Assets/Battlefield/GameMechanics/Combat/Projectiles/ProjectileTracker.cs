using System.Collections.Generic;

namespace Units.Abilities
{
    public class ProjectileTracker
    {
        Dictionary<Unit, List<TargetProjectile>> _targetProjectiles;


        public ProjectileTracker()
        {
            _targetProjectiles = new  Dictionary<Unit, List<TargetProjectile>>();
        }

        public void RegisterProjectile(TargetProjectile targetProjectile)
        {
            if (!_targetProjectiles.ContainsKey(targetProjectile.GetTargetUnit()))
            {
                _targetProjectiles[targetProjectile.GetTargetUnit()] = new List<TargetProjectile>();
            }
            _targetProjectiles[targetProjectile.GetTargetUnit()].Add(targetProjectile);
        }
        
        public void RegisterUnitDeath(Unit unit)
        {
            if (!_targetProjectiles.ContainsKey(unit))
            {
                return;
            }
            foreach (TargetProjectile targetProjectile in _targetProjectiles[unit].ToArray())
            {
                targetProjectile.ReactToUnitDeath(unit);
            }
            _targetProjectiles.Remove(unit);
            
        }

        public void UnregisterProjectile(TargetProjectile targetProjectile)
        {
            _targetProjectiles[targetProjectile.GetTargetUnit()].Remove(targetProjectile);
        }
    }
}