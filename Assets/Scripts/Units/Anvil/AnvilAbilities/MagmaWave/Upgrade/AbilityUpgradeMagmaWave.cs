using System;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;

namespace Units.Anvil.AnvilAbilities.MagmaWave
{
    public class AbilityUpgradeMagmaWave : IAbilityUpgrade
    {
        public float FlatBonusRadius { get; }
        public float IncreaseRadius{ get; }
        
        public float SpeedModifier { get; }
        
        public float FlatDamage { get; }
        public float IncreaseDamage { get; }
        
        public int Frequency { get; }
        
        private String _upgradeString;

        public AbilityUpgradeMagmaWave(
            float flatBonusRadius = 0f,
            float increaseRadius = 0f,
            float speedModifier = 0f,
            int flatDamage = 0,
            float increaseDamage = 0f,
            int frequency = 0,
            String upgradeString = null)
        {
            FlatBonusRadius = flatBonusRadius;
            IncreaseRadius = increaseRadius;
            SpeedModifier = speedModifier;
            FlatDamage = flatDamage;
            IncreaseDamage = increaseDamage;
            Frequency = frequency;
            _upgradeString = upgradeString;
        }

        public string GetUpgradeString()
        {
            return _upgradeString;
        }

        public AbilityId GetAbilityId()
        {
            return AbilityId.MagmaWave;
        }
    }
}