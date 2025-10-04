using System.Collections.Generic;
using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using UnityEngine;

namespace Units.GeneralUnit.Minion
{
    public class MinionUnit : MonoBehaviour, ITargetable
    {
        [SerializeField] private CircleFillOverlay fillOverlay;
        
        private Unit _unit;
        private Scheduler _scheduler;
       
        private BattlefieldInterfaceForUnit _battlefieldInterface;
        
        private AbilityManager _abilityManager;

        public void ApplySpawnInfo(
            BattlefieldInterfaceForUnit battlefieldInterface,
            Faction faction,
            Scheduler scheduler)
        {
            Debug.Log("ApplySpawnInfo");
            _scheduler = scheduler;
            _battlefieldInterface = battlefieldInterface;
            
            var singleTargetProjectileAbility = new SingleTargetProjectileAbility(
                transform, 
                faction, 
                4, 
                _scheduler,
                15,
                LayerMask.GetMask("Enemy"), 
                _battlefieldInterface);
            
            var singleTargetAbility = new Ability(singleTargetProjectileAbility);
            List<Ability> abilites = new List<Ability>();
            abilites.Add(singleTargetAbility);
            
            _abilityManager = new AbilityManager(abilites);
            
            _unit = new Unit(10,
                new DefaultAbilityModifierSetProducer(),
                Faction.Player,
                _battlefieldInterface,
                _abilityManager,
                fillOverlay,
                transform
                );
        }
        
        void OnDisable()
        {
            _unit.ManualOnDisable();
        }
        
        public void TakeDamage(int damage)
        {
            Debug.Log("MINION DAMAGE TAKING");
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
            return  transform.position;
        }

        public Unit GetUnit()
        {
            return _unit;
        }

        public Object Handle { get; }
    }
}