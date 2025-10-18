using System;
using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Resources;

namespace Units.Abilities.AbilityManagement
{
    public class AbilityManager
    {
        private List<Ability> _abilities;
        
        public void Init(AbilityModifierSet abilityModifierSet)
        {
            foreach (var ability in _abilities)
            {
                ability.Init(abilityModifierSet);
            }
        }
        
        public AbilityManager(List<Ability> abilities)
        {
            _abilities = abilities;
        }

        public void RefreshAbilityModifierSet(AbilityModifierSet abilityModifierSet)
        {
            foreach (var ability in _abilities)
            {
                ability.RefreshAbilityModifierSet(abilityModifierSet);
            }
        }

        public void ManualOnDisable()
        {
            foreach (var ability in _abilities)
            {
                ability.ManualOnDisable();
            }
        }
    }
}