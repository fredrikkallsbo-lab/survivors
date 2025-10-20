namespace Battlefield.GameMechanics.Combat.AbilityModifying
{
    public class DefaultAbilityModifierSetProducer : IAbilityModifierSetProducer
    {
        public AbilityModifierSet GetAbilityModifierSet()
        {
            return new AbilityModifierSet(0);
        }
    }
}