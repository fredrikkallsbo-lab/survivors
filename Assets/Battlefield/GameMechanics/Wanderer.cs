using Battlefield.GameMechanics.Combat.AbilityModifying;
using UnityEngine;

namespace Battlefield.GameMechanics
{
    public class Wanderer: IAbilityModifierSetProducer
    {
        private int _experience;

        
        public AbilityModifierSet GetAbilityModifierSet()
        {
            return new AbilityModifierSet(_experience/10);
        }

        public void AddExperience(int i)
        {
            _experience += i;
        }
    }
}