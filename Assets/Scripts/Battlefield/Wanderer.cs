using System.Collections.Generic;
using Battlefield.GameEvents;
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
        private int experiencePerLevel = 10;
        private IEventBus _eventBus;

        public Wanderer(Unit playerUnit, IEventBus eventBus)
        {
            _playerUnit = playerUnit;
            _eventBus = eventBus;
        }
        
        
        public void AddExperience(int i)
        {
            Debug.Log($"Adding experience {i}");
            _experience += i;
            CheckLevelup();
            _playerUnit.UpdateAbilityModifier(CreateAbilityModifier());
        }

        private void CheckLevelup()
        {
            if (_experience % experiencePerLevel == 0)
            {
                Debug.Log("Publishing level up event");
                _eventBus.Publish(new PlayerLevelUpEvent());
            }
        }

        public AbilityModifier CreateAbilityModifier()
        {
            return new AbilityModifier(_experience);
        }
    }
}