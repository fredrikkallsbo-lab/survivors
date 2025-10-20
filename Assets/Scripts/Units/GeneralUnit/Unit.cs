using System.Collections.Generic;
using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement;
using Units.Death;
using Units.GeneralUnit.DeathManagement;
using Units.HealthDisplay;
using Units.Resources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units
{
    public sealed class Unit : MonoBehaviour
    {
      
        private HealthTracker _healthTracker;

        public Faction faction;

        private BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;

        private AbilityManager _abilityManager;

        private Transform _unitTransform;

        private IHealthDisplayer _healthDisplayer;
        
        private UnitResourceManager _unitResourceManager;
        
        private TriggerManager  _triggerManager;
        
        private IEventBus _eventBus;
        
        private IDeathEventCreator _deathEventCreator;

        public void Init(int health,
            AbilityModifierSet abilityModifierSet,
            Faction faction,
            BattlefieldInterfaceForUnit battlefieldInterfaceForUnit,
            AbilityManager abilityManager,
            Transform unitTransform,
            IHealthDisplayer healthDisplayer,
            UnitResourceManager unitResourceManager,
            TriggerManager triggerManager,
            IEventBus bus,
            IDeathEventCreator deathEventCreator)
        {
            _healthTracker = new HealthTracker(health);
            _abilityManager = abilityManager;
            _battlefieldInterfaceForUnit = battlefieldInterfaceForUnit;
            this.faction = faction;
            _unitTransform = unitTransform;
            _healthDisplayer = healthDisplayer;
            _unitResourceManager = unitResourceManager;
            _triggerManager = triggerManager;
            _eventBus = bus;

            _battlefieldInterfaceForUnit.RegisterSpawn(this);
            _healthTracker.OnDied += HandleDeath;
            _deathEventCreator  = deathEventCreator;
            
            _abilityManager.Init(abilityModifierSet);
        }


        private void HandleDeath() => _battlefieldInterfaceForUnit.RegisterDeath(this);

      

        public void TakeDamage(int damage)
        {
            
            _healthTracker.TakeDamage(damage);
            _healthDisplayer.SetFill(_healthTracker.GetPercentageHealth());
            if (_healthTracker.IsDead())
            {
                
                _deathEventCreator.PublishDeathEvent(_eventBus);
                _battlefieldInterfaceForUnit.RegisterDeath(this);
            }
        }

        public void OnDisable()
        {
            _abilityManager.ManualOnDisable();
        }

        public Vector3 GetPosition()
        {
            return _unitTransform.position;
        }

        public void UpdateAbilityModifierSet(AbilityModifierSet abilityModifierSet)
        {
            _abilityManager.RefreshAbilityModifierSet(abilityModifierSet);
        }
    }
}