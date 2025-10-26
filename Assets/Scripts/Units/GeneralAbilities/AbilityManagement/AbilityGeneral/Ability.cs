using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;
using Units.Resources;

namespace Units.Abilities.AbilityManagement.AbilityGeneral
{
    public class Ability
    {
        private IAbilityEffect effect;

        public Ability(IAbilityEffect effect)
        {
            this.effect = effect;
        }

        public void Init(AbilityModifier abilityModifier)
        {
            effect.Init(abilityModifier);
        }

        public void RefreshAbilityModifierSet(AbilityModifier abilityModifierSet)
        {
            effect.RefreshAbilityModifier(abilityModifierSet);
        }

        public void ManualOnDisable()
        {
            effect.ManualOnDisable();
        }
    }
}