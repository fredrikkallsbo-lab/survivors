namespace Battlefield.GameMechanics.Combat.AbilityModifying
{
    public class AbilityModifierSet
    {
        private int _levels;

        private float _attackTime = 1f;

        public AbilityModifierSet(int levels)
        {
            _levels = levels;
        }

        public int Levels => _levels;

        public float GetAttackTime()
        {
            return _attackTime;
        }
    }
}