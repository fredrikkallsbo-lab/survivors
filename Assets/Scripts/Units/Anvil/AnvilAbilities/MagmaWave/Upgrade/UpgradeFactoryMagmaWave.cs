using System;
using System.Collections.Generic;
using System.Linq;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units.Anvil.AnvilAbilities.MagmaWave.Upgrade
{
    public class UpgradeFactoryMagmaWave
    {
        public enum MagmaWaveUpgradeTypes
        {
            FlatBonusRadius,
            IncreaseRadius,
            LowerSpeed,
            FasterSpeedAndFlatDamage,
            FlatDamage,
            IncreaseDamage,
        }
        
        private readonly Dictionary<MagmaWaveUpgradeTypes, float> _upgradeValues = new()
        {
            { MagmaWaveUpgradeTypes.FlatBonusRadius, 1f },
            { MagmaWaveUpgradeTypes.IncreaseRadius, 1f },
            { MagmaWaveUpgradeTypes.LowerSpeed, 1f },
            { MagmaWaveUpgradeTypes.FasterSpeedAndFlatDamage, 1f },
            { MagmaWaveUpgradeTypes.IncreaseDamage, 1f },
            { MagmaWaveUpgradeTypes.FlatDamage, 1f },
        };

        public AbilityUpgradeMagmaWave CreateUpgrade(MagmaWaveUpgradeTypes type)
        {
            switch (type)
            {
                case MagmaWaveUpgradeTypes.FlatBonusRadius:
                    return new AbilityUpgradeMagmaWave(flatBonusRadius: 5f, 
                        upgradeString: "Magma Wave: Add +5 to base radius");
                case MagmaWaveUpgradeTypes.IncreaseRadius:
                    return new AbilityUpgradeMagmaWave(increaseRadius: 1f, 
                        upgradeString: "Magma Wave: 100% increased radius");
                case MagmaWaveUpgradeTypes.LowerSpeed:
                    return new AbilityUpgradeMagmaWave(speedModifier: -0.25f, 
                        upgradeString: "Magma Wave: 25% reduced speed");
                case MagmaWaveUpgradeTypes.FasterSpeedAndFlatDamage:
                    return new AbilityUpgradeMagmaWave(speedModifier: 0.2f, 
                        increaseDamage: 0.5f,
                        upgradeString: "Magma Wave: Wave moves 20% faster and deals 50% increased damage");
                case MagmaWaveUpgradeTypes.FlatDamage:
                    return new AbilityUpgradeMagmaWave(flatDamage: 1, 
                        upgradeString: "Magma Wave: +1 base damage");
                case MagmaWaveUpgradeTypes.IncreaseDamage:
                    return new AbilityUpgradeMagmaWave(increaseDamage: 0.2f, 
                        upgradeString: "Magma Wave: 20% increased damage");
                default:
                    throw new ArgumentException("Unnaccounted for upgrade type");
            }
        }
        
        public List<MagmaWaveUpgradeTypes> GetAbilityUpgradeTypes(int n)
        {
            var candidates = new Dictionary<MagmaWaveUpgradeTypes, float>(_upgradeValues);

            List<MagmaWaveUpgradeTypes> upgrades = new List<MagmaWaveUpgradeTypes>();

            n = Mathf.Min(n, candidates.Count); 

            for (int i = 0; i < n; i++)
            {
                float totalWeight = candidates.Sum(c => c.Value);
                float pick = Random.value * totalWeight;

                float cumulative = 0f;
                foreach (var pair in candidates)
                {
                    cumulative += pair.Value;
                    if (pick <= cumulative)
                    {
                        upgrades.Add(pair.Key);
                        candidates.Remove(pair.Key); 
                        break;
                    }
                }
            }
            return upgrades;
        }
    }
    
    
}