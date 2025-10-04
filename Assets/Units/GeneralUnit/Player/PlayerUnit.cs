using System;
using System.Collections.Generic;
using Battlefield;
using Battlefield.GameMechanics;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Units.Player
{
    public class PlayerUnit: MonoBehaviour, ITargetable
    {
        [SerializeField] private Scheduler scheduler;
        [SerializeField] private BattlefieldController battlefieldController;
        [SerializeField] private CircleFillOverlay fillOverlay;
        
        private Unit _unit;
        private Character _character;
        private AbilityManager _abilityManager;
        
        
        void Awake()
        {
            var singleTargetClosestAbility = new SingleTargetClosestAbility(
                transform, 
                Faction.Player, 
                2, 
                scheduler,
                10,
                LayerMask.GetMask("Enemy"));
            
            
            
            var singleTargetAbility = new Ability(singleTargetClosestAbility);
            List<Ability> abilites = new List<Ability>();
            //abilites.Add(singleTargetAbility);
            
            _abilityManager = new AbilityManager(abilites);
            
            _character = new Character();
            
            _unit = new Unit(
                100, 
                _character, 
                Faction.Player, 
                battlefieldController.GetBattlefieldKnowledge(),
                _abilityManager,
                fillOverlay,
                transform
                );
        }

        private void OnEnable()
        {
            _unit.ManualOnEnable(_character.GetAbilityModifierSet());
            _abilityManager.Init(_character.GetAbilityModifierSet());
        }

        void OnDisable()
        {
            _unit.ManualOnDisable();
        }

        public void RefreshAbilityModifierSet()
        {
            _unit.RefreshAbilityModifierSet();
        }

        public void TakeDamage(int damage)
        {
            if (_unit.TakeDamage(damage))
            {
                Destroy(gameObject);
            }
        }

        public Faction GetFaction()
        {
            return _unit.Faction;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Unit GetUnit()
        {
            return _unit;
        }

        public Object Handle { get; }
    }
}