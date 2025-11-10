using System;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;
using Units.GeneralAbilities.AbilityManagement.AbilityUpgrades;

namespace Units.Anvil.AnvilAbilities.MagmaWave.Upgrade
{
    public class AbilityUpgradePacketMagmaWave : IAbilityUpgradePacket
    {
        public float FlatBonusRadius { get; private set; }
        public float IncreaseRadius{ get; private set; }
        
        public float SpeedModifier { get; private set; }
        
        public float FlatDamage { get; private set; }
        public float IncreaseDamage { get; private set; }
        
        public int Frequency { get; private set; }

        public AbilityUpgradePacketMagmaWave()
        {
            FlatBonusRadius = 0;
            IncreaseRadius = 1;
            SpeedModifier = 1;
            FlatDamage = 0;
            IncreaseDamage = 1;
        }
        
        public void ApplyUpgrade(IAbilityUpgrade upgrade)
        {
            if (upgrade.GetAbilityId() != AbilityId.MagmaWave)
            {
                throw new Exception("AbilityUpgradePacketMagmaWave must be magma wave");
            }

            AbilityUpgradeMagmaWave magmaWaveUpgrade = upgrade as AbilityUpgradeMagmaWave;
            FlatBonusRadius += magmaWaveUpgrade.FlatBonusRadius;
            IncreaseRadius += magmaWaveUpgrade.IncreaseRadius;
            SpeedModifier += magmaWaveUpgrade.SpeedModifier;
            FlatDamage += magmaWaveUpgrade.FlatDamage;
            IncreaseDamage += magmaWaveUpgrade.IncreaseDamage;
            Frequency += magmaWaveUpgrade.Frequency;
        }
    }
}