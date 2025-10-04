using Battlefield.GameMechanics.Combat.AbilityModifying;
using UnityEngine;

namespace Battlefield.GameMechanics
{
    public class Character: IAbilityModifierSetProducer
    {
        private int level;

        public void RegisterDeath()
        {
            Debug.Log("Character| level up");
            level++;
        }


        public AbilityModifierSet GetAbilityModifierSet()
        {
            return new AbilityModifierSet(level);
        }
    }
}