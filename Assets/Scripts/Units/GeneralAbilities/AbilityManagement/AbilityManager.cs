using System;
using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;
using Units.Resources;

namespace Units.Abilities.AbilityManagement
{
    public class AbilityManager
    {
        private List<IAbility> _abilities;
        
        public void Init(AbilityModifier abilityModifier)
        {
            foreach (var ability in _abilities)
            {
                ability.Init(abilityModifier);
            }
        }
        
        public AbilityManager(List<IAbility> abilities)
        {
            _abilities = abilities;
        }

        public void RefreshAbilityModifier(AbilityModifier abilityModifier)
        {
            foreach (var ability in _abilities)
            {
                ability.RefreshAbilityModifierSet(abilityModifier);
            }
        }

        public void ManualOnDisable()
        {
            foreach (var ability in _abilities)
            {
                ability.ManualOnDisable();
            }
        }

        public List<IAbilityUpgrade> GetUpgrades(int numberOfUpgrades)
        {
            List<IAbilityUpgrade> upgrades = new List<IAbilityUpgrade>();

            foreach (var ability in _abilities)
            {
                if (ability.GetAbilityId() == AbilityId.MagmaWave)
                {
                    upgrades.AddRange(ability.GetUpgrades(numberOfUpgrades));
                }
            }
            return upgrades;
        }
    }
}