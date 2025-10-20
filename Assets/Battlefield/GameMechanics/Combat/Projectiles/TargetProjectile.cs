using Battlefield;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using UnityEngine;

namespace Units.Abilities
{
    public class TargetProjectile: MonoBehaviour
    {
        private int _damage;
        private float _speed;
        private Unit _targetUnit;
        private Vector2 _lastKnownPosition;
        BattlefieldInterfaceForUnit _battlefieldController;
        Vector3? _finalDestination; 


        public void Init(int damage, float speed, Unit target, BattlefieldInterfaceForUnit battlefieldController)
        {
            _damage = damage;
            _speed = speed;
            _targetUnit = target;
            _battlefieldController = battlefieldController;
        }

        void FixedUpdate()
        {
            // If the target exists, keep updating the last known position.
            if (_targetUnit != null)
            {
                _lastKnownPosition = _targetUnit.GetPosition();
            }
            else if (_finalDestination == null)
            {
                // Target just got lost — lock a final destination once.
                _finalDestination = _lastKnownPosition;
            }

            // Choose where to go this frame
            Vector3 destination = _finalDestination ?? _lastKnownPosition;

            float step = _speed * Time.fixedDeltaTime;
            Vector3 next = Vector3.MoveTowards(transform.position, destination, step);

            // Move (use rb if present to play nicely with physics)
            
            transform.position = next;

            // Arrived?
            // Use a small epsilon to avoid float jitter.
            if ((destination - next).sqrMagnitude <= 1e-6f)
            {
                if (_finalDestination != null)    // we were finishing travel after loss
                {
                    Destroy(gameObject);
                }
                else
                {
                    // Target still alive: trigger the impact logic
                    Debug.Log("Trigger projectile impact");
                    TriggerProjectileImpact();
                }
            }
        }

        private void TriggerProjectileImpact()
        {
           _targetUnit.TakeDamage(_damage);
           Destroy(gameObject);
        }

        public Unit GetTargetUnit()
        {
            return _targetUnit;
        }

        public void ReactToUnitDeath(Unit unit)
        {
            Destroy(gameObject);
        }
    }
    
    
}