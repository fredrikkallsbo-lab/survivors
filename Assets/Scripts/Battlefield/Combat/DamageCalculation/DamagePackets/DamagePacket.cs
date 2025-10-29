using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Battlefield.Combat.DamageCalculation.DamagePackets
{
    public class DamagePacket
    {
        private Dictionary<DamageType, int> _damage =  new();

        public DamagePacket()
        {
        }

        public DamagePacket(Dictionary<DamageType, int> damage)
        {
            _damage = damage;
        }

        public void AddDamage(DamageType damageType, int damage)
        {
            if (!_damage.TryAdd(damageType, damage))
            {
                _damage[damageType] += damage;
            }
        }
        
        public DamagePacket CreateCopy()
        {
            return new DamagePacket(new Dictionary<DamageType, int>(_damage));
        }

        public int GetTotalDamage()
        {
            int totalDamage = 0;
            foreach (DamageType damageType in _damage.Keys)
            {
                totalDamage += _damage[damageType];
            }
            return totalDamage;
        }
    }
}