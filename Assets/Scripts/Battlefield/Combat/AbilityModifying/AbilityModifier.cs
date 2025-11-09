using System.Collections.Generic;
using Battlefield.Combat.DamageCalculation.DamagePipeline;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using Units.Anvil.AnvilAbilities.MagmaWave.Upgrade;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;
using Units.GeneralAbilities.AbilityManagement.AbilityUpgrades;

namespace Battlefield.GameMechanics.Combat.AbilityModifying
{
    public class AbilityModifier
    {
        private readonly int _levels;

        private readonly float _attackTime = 1f;
        
        private DamagePipeline _damagePipeline;
        
        private Dictionary<AbilityId, IAbilityUpgradePacket>  _abilityUpgradePackets;

        public AbilityModifier(int levels)
        {
            _levels = levels;
            _abilityUpgradePackets = new Dictionary<AbilityId, IAbilityUpgradePacket>();
            _abilityUpgradePackets[AbilityId.MagmaWave] = new AbilityUpgradePacketMagmaWave();
        }

        public int Levels => _levels;

        public float GetAttackTime()
        {
            return _attackTime;
        }

        public IAbilityUpgradePacket GetAbilityUpgradePacket(AbilityId abilityId)
        {
            return _abilityUpgradePackets[abilityId];
        }


        public void AddUpgrades(List<IAbilityUpgrade> abilityUpgradeList)
        {
            foreach (IAbilityUpgrade upgrade in abilityUpgradeList)
            {
                _abilityUpgradePackets[upgrade.GetAbilityId()].ApplyUpgrade(upgrade);
            }
        }
    }
}