using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;
using Units.Resources;

namespace Units.Abilities.AbilityManagement.AbilityGeneral
{
    public interface IAbility
    {

        public void Init(AbilityModifier abilityModifier);

        public void RefreshAbilityModifierSet(AbilityModifier abilityModifierSet);

        public void ManualOnDisable();

        public AbilityId GetAbilityId();

        public List<IAbilityUpgrade> GetUpgrades(int numberOfUpgrades);
        
    }
}