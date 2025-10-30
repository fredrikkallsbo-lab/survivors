using System.Collections.Generic;
using Battlefield;
using Battlefield.Combat.StatusEffectManagement;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Battlefield.GameMechanics.Combat.BuffManagement;
using Units.Abilities.AbilityManagement;
using Units.Anvil.AnvilAbilities;
using Units.Death;
using Units.HealthDisplay;
using Units.Resources;
using UnityEngine;

namespace Units
{
    public sealed class Unit : MonoBehaviour
    {
      
        private HealthTracker _healthTracker;

        public Faction faction;

        private BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;

        private AbilityManager _abilityManager;
        

        private IHealthDisplayer _healthDisplayer;
        
        private UnitResourceManager _unitResourceManager;
        
        private TriggerManager  _triggerManager;
        
        private IEventBus _eventBus;
        
        private IDeathEventCreator _deathEventCreator;
        
        private StatusEffectManager  _statusEffectManager;
        public void Init(int health,
            AbilityModifier abilityModifier,
            Faction faction,
            BattlefieldInterfaceForUnit battlefieldInterfaceForUnit,
            AbilityManager abilityManager,
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
            _healthDisplayer = healthDisplayer;
            _unitResourceManager = unitResourceManager;
            _triggerManager = triggerManager;
            _eventBus = bus;

            _battlefieldInterfaceForUnit.RegisterSpawn(this);
            _deathEventCreator  = deathEventCreator;
            
            _abilityManager.Init(abilityModifier);
            _statusEffectManager = new StatusEffectManager(this);
        }



      

        public void TakeDamage(int damage)
        {
            _healthTracker.TakeDamage(damage);
            Debug.Log("Taking damage unit:" + name + ", new health: " + _healthTracker.CurrentHp);
            _healthDisplayer.SetFill(_healthTracker.GetPercentageHealth());
            if (_healthTracker.IsDead())
            {
                _deathEventCreator.PublishDeathEvent(_eventBus, this);
                _battlefieldInterfaceForUnit.RegisterDeath(this);
            }
        }

        public void OnDisable()
        {
            _abilityManager.ManualOnDisable();
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void UpdateAbilityModifier(AbilityModifier abilityModifier)
        {
            _abilityManager.RefreshAbilityModifier(abilityModifier);
        }

        public void ReceiveStatusEffect(IStatusEffect statusEffect)
        {
            _statusEffectManager.AddStatusEffect(statusEffect);
            statusEffect.ApplyStatusEffect(this);
        }

        public void AddTrigger(ITrigger trigger)
        {
            _triggerManager.Add(trigger);
        }

        public void RemoveTrigger(ITrigger trigger)
        {
            _triggerManager.Remove(trigger);
        }

        public bool HasStatusEffect(StatusEffectId statusEffectId)
        {
            return _statusEffectManager.HasStatusEffect(statusEffectId);
        }
    }
}