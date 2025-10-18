using Battlefield.GameMechanics.Combat.AbilityModifying;
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

        public void Init(AbilityModifierSet abilityModifierSet)
        {
            effect.Init(abilityModifierSet);
        }

        public void RefreshAbilityModifierSet(AbilityModifierSet abilityModifierSet)
        {
            effect.RefreshAbilityModifierSet(abilityModifierSet);
        }

        public void ManualOnDisable()
        {
            effect.ManualOnDisable();
        }
    }
}