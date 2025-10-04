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
        private Faction _sourceFaction;
        
        private System.Action _cancel;
        
        public AbilityModifierSet _abilityModifierSet;

        private Transform _sourceTransform;

        private int _baseDamage = 2;
        
        private Scheduler _scheduler;
        
        
        public SingleTargetClosestAbility(Transform sourceTransform, 
            Faction sourceFaction, 
            int baseDamage, 
            Scheduler scheduler,
            float radius,
            int layerMask)
        {
            _sourceTransform = sourceTransform;
            _sourceFaction = sourceFaction;
            _baseDamage = baseDamage;
            _scheduler = scheduler;
            _radius = radius;
            _layerMask = layerMask;
            
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
            Debug.Log("Attack");
            float closestDistance = Mathf.Infinity;
            Collider2D closestCollider = null;
            
            Collider2D[] colliders = new Collider2D[100];
            var size = Physics2D.OverlapCircleNonAlloc(_sourceTransform.position, _radius, colliders, _layerMask);
            for (int i = 0; i < size; i++)
            {
                Collider2D collider = colliders[i];
                if ((closestDistance > Vector2.Distance(collider.transform.position, _sourceTransform.position)
                    && collider.GetComponent<ITargetable>().GetFaction() != _sourceFaction))
                {
                    closestDistance = Vector2.Distance(collider.transform.position, _sourceTransform.position);
                    closestCollider = collider;
                }
            }

            if (closestCollider != null)
            {
                Debug.Log("Attacking: "+ closestCollider.GetComponent<ITargetable>().GetFaction() + " from: " + _sourceFaction);
                closestCollider.GetComponent<ITargetable>().TakeDamage(_baseDamage + _abilityModifierSet.Levels);
            }
        }
        
    }
}