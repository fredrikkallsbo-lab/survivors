using Battlefield.GameMechanics.Combat.AbilityModifying;

namespace Units.Abilities.AbilityManagement.AbilityGeneral
{
    public interface IAbilityEffect
    {

        public void Init(AbilityModifierSet abilityModifierSet);
        
        public void RefreshAbilityModifierSet(AbilityModifierSet abilityModifierSet);
        
        public void ManualOnDisable();
    }
}