using Battlefield.GameMechanics.Combat.AbilityModifying;

namespace Units.GeneralAbilities.AbilityManagement.AbilityGeneral
{
    public interface IAbilityEffect
    {

        public void Init(AbilityModifier abilityModifier);
        
        public void RefreshAbilityModifier(AbilityModifier abilityModifier);
        
        public void ManualOnDisable();
    }
}