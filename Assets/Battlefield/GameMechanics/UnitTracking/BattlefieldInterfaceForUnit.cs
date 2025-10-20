using System;
using Battlefield.GameMechanics;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units;
using Units.Abilities;
using UnityEngine;

namespace Battlefield
{
    public class BattlefieldInterfaceForUnit
    {
        private BattlefieldController _battlefieldController;
       
        
        public BattlefieldInterfaceForUnit(BattlefieldController battlefieldController)
        {
            _battlefieldController = battlefieldController;
        }

        public void RegisterSpawn(Unit unit)
        {
            _battlefieldController.RegisterUnit(unit);
        }
        
        public void RegisterDeath(Unit unit)
        {
            _battlefieldController.UnregisterUnit(unit);
        }

        public Unit GetClosestUnitOfFaction(Faction faction, Transform sourceTransform, float radius, int layerMask)
        {
            float closestDistance = Mathf.Infinity;
            Collider2D closestCollider = null;
            
            Collider2D[] colliders = new Collider2D[100];
            var size = Physics2D.OverlapCircleNonAlloc(sourceTransform.position, radius, colliders, layerMask);
            for (int i = 0; i < size; i++)
            {
                Collider2D collider = colliders[i];
                if (closestDistance > Vector2.Distance(collider.transform.position, sourceTransform.position)
                     && collider.GetComponent<Unit>().Faction == faction)
                {
                    closestDistance = Vector2.Distance(collider.transform.position, sourceTransform.position);
                    closestCollider = collider;
                }
            }

            if (closestCollider != null)
            {
                Debug.Log("Returning Unit"); 
                return closestCollider.GetComponent<Unit>();
            }

            return null;
        }


        public void RegisterWanderer(Wanderer wanderer)
        {
            _battlefieldController.RegisterWanderer(wanderer);
        }
    }
}