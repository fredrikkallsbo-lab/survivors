using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement;
using Units.HealthDisplay;
using UnityEngine;

namespace Units
{
    public sealed class Unit : MonoBehaviour
    {
        private IAbilityModifierSetProducer _iAbilityModifierSetProducer;
        private HealthTracker _healthTracker;

        public Faction Faction;

        private BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;

        private AbilityManager _abilityManager;

        private Transform _unitTransform;

        private IHealthDisplayer _healthDisplayer;

        public void Init(int health,
            IAbilityModifierSetProducer abilityModifierSetProducer,
            Faction faction,
            BattlefieldInterfaceForUnit battlefieldInterfaceForUnit,
            AbilityManager abilityManager,
            Transform unitTransform,
            IHealthDisplayer healthDisplayer)
        {
            _healthTracker = new HealthTracker(health);
            _abilityManager = abilityManager;
            _iAbilityModifierSetProducer = abilityModifierSetProducer;
            _battlefieldInterfaceForUnit = battlefieldInterfaceForUnit;
            Faction = faction;
            _unitTransform = unitTransform;
            _healthDisplayer = healthDisplayer;

            _abilityManager.Init(_iAbilityModifierSetProducer.GetAbilityModifierSet());

            _battlefieldInterfaceForUnit.RegisterSpawn(this);
            _healthTracker.OnDied += HandleDeath;
            
            _abilityManager.Init(_iAbilityModifierSetProducer.GetAbilityModifierSet());
        }


        private void HandleDeath() => _battlefieldInterfaceForUnit.RegisterDeath(this);

        public void RefreshAbilityModifierSet()
        {
            _abilityManager.RefreshAbilityModifierSet(_iAbilityModifierSetProducer.GetAbilityModifierSet());
        }

        public void TakeDamage(int damage)
        {
            
            _healthTracker.TakeDamage(damage);
            _healthDisplayer.SetFill(_healthTracker.GetPercentageHealth());
            if (_healthTracker.IsDead())
            {
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
    }
}