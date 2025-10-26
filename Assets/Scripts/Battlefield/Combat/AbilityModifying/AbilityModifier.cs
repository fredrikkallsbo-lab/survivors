using Battlefield.Combat.DamageCalculation.DamagePipeline;

namespace Battlefield.GameMechanics.Combat.AbilityModifying
{
    public class AbilityModifier
    {
        private readonly int _levels;

        private readonly float _attackTime = 1f;
        
        private DamagePipeline _damagePipeline;

        public AbilityModifier(int levels)
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