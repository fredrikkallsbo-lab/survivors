using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Battlefield.GameMechanics
{
    public class Wanderer
    {
        private int _experience;
        private Unit _playerUnit;

        public Wanderer(Unit playerUnit)
        {
            _playerUnit = playerUnit;
        }
        
        
        public void AddExperience(int i)
        {
            _experience += i;
            _playerUnit.UpdateAbilityModifierSet(CreateAbilityModifierSet());
        }

        public AbilityModifierSet CreateAbilityModifierSet()
        {
            return new AbilityModifierSet(_experience);
        }
    }
}