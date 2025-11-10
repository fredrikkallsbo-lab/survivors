using System.Collections.Generic;
using Battlefield.GameEvents;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using Units.GeneralUnit.Minion;

namespace Battlefield.GameMechanics
{
    public class Wanderer
    {
        private int _experience;
        private Unit _playerUnit;
        private int experiencePerLevel = 2;
        private IEventBus _eventBus;

        private List<IAbilityUpgrade> _abilityUpgradeList;
        
        public Wanderer(Unit playerUnit, IEventBus eventBus)
        {
            _playerUnit = playerUnit;
            _eventBus = eventBus;
            _abilityUpgradeList = new List<IAbilityUpgrade>();
        }
        
        
        public void AddExperience(int i)
        {
            _experience += i;
            CheckLevelup();
            UpdateAbilityModifier();
        }

        private void CheckLevelup()
        {
            if (_experience % experiencePerLevel == 0)
            {
                _eventBus.Publish(new PlayerLevelUpEvent());
            }
        }

        public AbilityModifier CreateAbilityModifier()
        {
            return new AbilityModifier(_experience);
        }

        public List<IAbilityUpgrade> GetAbilityLevelUpOptions(int amountOfOptions)
        {
            AbilityManager abilityManager = _playerUnit.GetAbilityManager();

            return abilityManager.GetUpgrades(amountOfOptions);
        }

        public void AddUpgrade(IAbilityUpgrade upgrade)
        {
            _abilityUpgradeList.Add(upgrade);
            UpdateAbilityModifier();
        }

        private void UpdateAbilityModifier()
        {
            AbilityModifier abilityModifier = CreateAbilityModifier();
            abilityModifier.AddUpgrades(_abilityUpgradeList);
            _playerUnit.UpdateAbilityModifier(abilityModifier);
        }
    }
}