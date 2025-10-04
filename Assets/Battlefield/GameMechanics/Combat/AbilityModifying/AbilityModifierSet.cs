namespace Battlefield.GameMechanics.Combat.AbilityModifying
{
    public class AbilityModifierSet
    {
        private int _levels;

        public AbilityModifierSet(int levels)
        {
            _levels = levels;
        }

        public int Levels => _levels;
    }
}