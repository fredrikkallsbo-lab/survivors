using System;
using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Resources;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Units.Abilities
{
    public class SingleTargetProjectileAbility : IAbilityEffect
    {
        private GameObject _projectilePrefab;
        
        private float _radius;
        int _layerMask;
        private Faction _sourceFaction;
        
        private Action _cancel;
        
        public AbilityModifierSet _abilityModifierSet;

        private Transform _sourceTransform;

        private int _baseDamage = 2;
        
        private Scheduler _scheduler;
        
        private BattlefieldInterfaceForUnit _battlefieldInterface;
        
        
        public SingleTargetProjectileAbility(Transform sourceTransform, 
            Faction sourceFaction, 
            int baseDamage, 
            Scheduler scheduler,
            float radius,
            int layerMask,
            BattlefieldInterfaceForUnit battlefieldInterface)
        {
            _sourceTransform = sourceTransform;
            _sourceFaction = sourceFaction;
            _baseDamage = baseDamage;
            _scheduler = scheduler;
            _radius = radius;
            _projectilePrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/Projectile");
            _layerMask = layerMask;
            _battlefieldInterface = battlefieldInterface;
        }
        
        public void Init(AbilityModifierSet abilityModifierSet)
        {
            _abilityModifierSet = abilityModifierSet;
            _cancel = _scheduler.Every(1.0, Attack);
        }

        public void RefreshAbilityModifierSet(AbilityModifierSet abilityModifierSet)
        {
            _abilityModifierSet = abilityModifierSet;
        }

        public void ManualOnDisable()
        {
            _cancel?.Invoke();
        }
        
        public void Attack()
        {

            Unit targetUnit = _battlefieldInterface.GetClosestUnitOfFaction(Faction.Enemy, _sourceTransform, 100, _layerMask);
            if (targetUnit != null)
            {
                Debug.Log("Projectile attack");

                SendProjectile(targetUnit);
            }
        }

        private TargetProjectile SendProjectile(Unit targetUnit)
        {
            Vector3 spawnPos = _sourceTransform.position;        

            GameObject projectile = Object.Instantiate(_projectilePrefab, spawnPos, Quaternion.identity);
            projectile.GetComponent<TargetProjectile>().Init(_baseDamage, 2.5f, targetUnit, _battlefieldInterface);
            return projectile.gameObject.GetComponent<TargetProjectile>();
        }
    }
}