using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;

namespace Units.Player.PlayerAbilities
{
    public class AbilityEffectSpawnMinion : IAbilityEffect
    {
        private int _minionHealth;
        private int _minionDamage;
        private int _minionAttackRange;
        private int _minionSpawnRange;
        
        
        public void Init(AbilityModifierSet abilityModifierSet)
        {
            throw new System.NotImplementedException();
        }

        public void RefreshAbilityModifierSet(AbilityModifierSet abilityModifierSet)
        {
            throw new System.NotImplementedException();
        }

        public void ManualOnDisable()
        {
            throw new System.NotImplementedException();
        }
    }
}