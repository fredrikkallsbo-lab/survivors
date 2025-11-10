using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using Units.Anvil.AnvilAbilities.MagmaWave.Upgrade;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;

namespace Units.Anvil.AnvilAbilities.MagmaWave
{
    public class AbilityMagmaWave : IAbility
    {
        private static readonly AbilityId AbilityId = AbilityId.MagmaWave;
     
        private AbilityEffectMagmaWave _abilityEffectMagmaWave;
        private UpgradeFactoryMagmaWave _upgradeFactoryMagmaWave;

        public AbilityMagmaWave(TriggerManager triggerManager, IEventBus eventBus, AnvilSpellcaster spellcaster)
        {
            _abilityEffectMagmaWave =  new AbilityEffectMagmaWave(triggerManager, eventBus, spellcaster);
            _upgradeFactoryMagmaWave = new UpgradeFactoryMagmaWave();
        }
        
        public void Init(AbilityModifier abilityModifier)
        {
            _abilityEffectMagmaWave.Init(abilityModifier);
        }

        public void RefreshAbilityModifierSet(AbilityModifier abilityModifierSet)
        {
            _abilityEffectMagmaWave.RefreshAbilityModifier(abilityModifierSet);
        }

        public void ManualOnDisable()
        {
           _abilityEffectMagmaWave.ManualOnDisable();
        }

        public AbilityId GetAbilityId()
        {
            return AbilityId;
        }

        public List<IAbilityUpgrade> GetUpgrades(int numberOfUpgrades)
        {
            List<UpgradeFactoryMagmaWave.MagmaWaveUpgradeTypes> types = 
                _upgradeFactoryMagmaWave.GetAbilityUpgradeTypes(numberOfUpgrades);
            List<IAbilityUpgrade> upgrades = new List<IAbilityUpgrade>();
            foreach (UpgradeFactoryMagmaWave.MagmaWaveUpgradeTypes type in types)
            {
                upgrades.Add(_upgradeFactoryMagmaWave.CreateUpgrade(type));
            }

            return upgrades;
        }
        
    }
}