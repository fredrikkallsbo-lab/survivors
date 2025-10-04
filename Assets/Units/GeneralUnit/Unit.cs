using System.Collections.Generic;
using Battlefield;
using Battlefield.GameMechanics;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using UnityEngine;

namespace Units
{
    
    
    public sealed class Unit
    {
        private readonly IAbilityModifierSetProducer _iAbilityModifierSetProducer;
        private readonly HealthTracker _healthTracker;

        public readonly Faction Faction;

        private readonly BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;

        private AbilityManager _abilityManager;
        
        private Transform _unitTransform;

        public Unit(int health, 
            IAbilityModifierSetProducer abilityModifierSetProducer,
            Faction faction, 
            BattlefieldInterfaceForUnit battlefieldInterfaceForUnit,
            AbilityManager abilityManager,
            CircleFillOverlay fillOverlay,
            Transform unitTransform)
        {
            _healthTracker = new HealthTracker(health, fillOverlay);
            _abilityManager = abilityManager;
            _iAbilityModifierSetProducer = abilityModifierSetProducer;
            _battlefieldInterfaceForUnit = battlefieldInterfaceForUnit;
            Faction = faction;
            _unitTransform = unitTransform;
            
            _abilityManager.Init(_iAbilityModifierSetProducer.GetAbilityModifierSet());

            _battlefieldInterfaceForUnit.RegisterSpawn(this);
            _healthTracker.OnDied += HandleDeath; 
        }


        private void HandleDeath() => _battlefieldInterfaceForUnit.RegisterDeath(this);

        public void RefreshAbilityModifierSet()
        {
            _abilityManager.RefreshAbilityModifierSet(_iAbilityModifierSetProducer.GetAbilityModifierSet());
        }

        
        
        public bool TakeDamage(int damage)
        {
            _healthTracker.TakeDamage(damage);
            return _healthTracker.IsDead();
        }


        public void ManualOnEnable(AbilityModifierSet abilityModifierSet)
        {
            //måste initta alla spells
            _abilityManager.Init(abilityModifierSet);
        }
        public void ManualOnDisable()
        {
            _abilityManager.ManualOnDisable();
        }

        public Vector2 GetPosition()
        {
            return _unitTransform.position;
        }
    }
}