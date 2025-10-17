using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using UnityEngine;

namespace Units.Abilities
{
    public class SingleTargetClosestAbility: IAbilityEffect
    {
        
        private float _radius;
        int _layerMask;
        private Faction _targetFaction;
        
        private System.Action _cancel;
        
        public AbilityModifierSet _abilityModifierSet;

        private Transform _sourceTransform;

        private int _baseDamage = 2;
        
        private Scheduler _scheduler;
        
        private BattlefieldInterfaceForUnit _battlefieldInterface;
        
        
        public SingleTargetClosestAbility(Transform sourceTransform, 
            Faction targetFaction, 
            int baseDamage, 
            Scheduler scheduler,
            float radius,
            int layerMask,
            BattlefieldInterfaceForUnit battlefieldInterface)
        {
            _sourceTransform = sourceTransform;
            _targetFaction = targetFaction;
            _baseDamage = baseDamage;
            _scheduler = scheduler;
            _radius = radius;
            _layerMask = layerMask;
            _battlefieldInterface = battlefieldInterface;
        }

        public void Init(AbilityModifierSet abilityModifierSet)
        {
            Debug.Log("SingleTargetClosestAbility.Init|");
            _abilityModifierSet = abilityModifierSet;
            _cancel = _scheduler.Every(1.0, Attack);
        }

        public void ManualOnDisable()
        {
            _cancel?.Invoke();
        }

        public void RefreshAbilityModifierSet(AbilityModifierSet abilityModifierSet)
        {
            _abilityModifierSet = abilityModifierSet;
        }

        public void Attack()
        {
            Unit targetUnit =
                _battlefieldInterface.GetClosestUnitOfFaction(_targetFaction, _sourceTransform, _radius, _layerMask);
            if (targetUnit != null)
            {
                targetUnit.TakeDamage(_baseDamage + _abilityModifierSet.Levels);
            }
        }
    }
}