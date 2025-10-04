using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using UnityEngine;

namespace Units.Abilities
{
    public class SingleTargetProjectileAbility : IAbilityEffect
    {
        private GameObject _projectilePrefab;
        
        private float _radius;
        int _layerMask;
        private Faction _sourceFaction;
        
        private System.Action _cancel;
        
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
            _projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
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
            Debug.Log("Attack projectile");
            float closestDistance = Mathf.Infinity;
            Collider2D closestCollider = null;
            
            Collider2D[] colliders = new Collider2D[100];
            var size = Physics2D.OverlapCircleNonAlloc(_sourceTransform.position, _radius, colliders, _layerMask);
            for (int i = 0; i < size; i++)
            {
                Collider2D collider = colliders[i];
                if (closestDistance > Vector2.Distance(collider.transform.position, _sourceTransform.position)
                     && collider.GetComponent<ITargetable>().GetFaction() != _sourceFaction)
                {
                    closestDistance = Vector2.Distance(collider.transform.position, _sourceTransform.position);
                    closestCollider = collider;
                }
            }

            if (closestCollider != null)
            {
                Debug.Log("Attacking: "+ closestCollider.GetComponent<ITargetable>().GetFaction() + " from: " + _sourceFaction);
                _battlefieldInterface.RegisterProjectile(SendProjectile(closestCollider.GetComponent<ITargetable>()));
            }
        }

        private TargetProjectile SendProjectile(ITargetable target)
        {
            Vector3 spawnPos = _sourceTransform.position;        
            Quaternion spawnRot = _sourceTransform.rotation;

            GameObject projectile = Object.Instantiate(_projectilePrefab, spawnPos, spawnRot);
            projectile.GetComponent<TargetProjectile>().Init(_baseDamage, 1, target, _battlefieldInterface);
            return projectile.gameObject.GetComponent<TargetProjectile>();
        }
    }
}