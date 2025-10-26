namespace Battlefield.GameMechanics.Combat.AbilityModifying
{
    public class DefaultAbilityModifierProducer : IAbilityModifierProducer
    {
        public AbilityModifier GetAbilityModifier()
        {
            return new AbilityModifier(0);
        }
    }
}