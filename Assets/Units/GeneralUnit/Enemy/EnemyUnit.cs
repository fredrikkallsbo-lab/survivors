using System;
using System.Collections.Generic;
using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Units.Enemy
{
    public class EnemyUnit : MonoBehaviour, ITargetable
    {

        [SerializeField] private CircleFillOverlay _fillOverlay;
        
        private Unit _unit;
        private Scheduler _scheduler;
       
        private BattlefieldInterfaceForUnit _knowledge;
        
        private AbilityManager _abilityManager;

        private const Faction Faction = global::Faction.Enemy;

        public void ApplySpawnInfo(int health, 
            IAbilityModifierSetProducer abilityModifierSetProducer,
            BattlefieldInterfaceForUnit knowledge,
            Scheduler scheduler)
        {
            _knowledge = knowledge;
            _scheduler = scheduler;
            
            var singleTargetClosestAbility = new SingleTargetClosestAbility(
                transform, 
                Faction, 
                2, 
                _scheduler,
                15,
                LayerMask.GetMask("Ally"));
            
            var singleTargetAbility = new Ability(singleTargetClosestAbility);
            List<Ability> abilites = new List<Ability>();
            abilites.Add(singleTargetAbility);
            
            _abilityManager = new AbilityManager(abilites);
            _unit = new Unit(health,
                abilityModifierSetProducer,
                Faction,
                _knowledge,
                _abilityManager,
                _fillOverlay,
                transform);
        }
        


        void OnDisable()
        {
            _unit.ManualOnDisable();
        }

        public void TakeDamage(int damage)
        {
            Debug.Log("ENEMY TAKING DAMAGE");
            if (_unit.TakeDamage(damage))
            {
                Debug.Log("Dead");
                _knowledge.RegisterDeath(_unit);
                Destroy(gameObject);
            }
        }

        public Faction GetFaction()
        {
            return Faction;
        }

        public Vector3 GetPosition()
        {
            return  transform.position;
        }

        public Unit GetUnit()
        {
            return _unit;
        }

        public Object Handle { get; }
    }
    
}