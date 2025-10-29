using Battlefield;
using UnityEngine;

namespace Units.GeneralUnit.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MeleeChase : MonoBehaviour
    {
        private BattlefieldInterfaceForUnit _battlefieldInterface;
        private Rigidbody2D _rb;
        private Faction _targetFaction;
        private LayerMask _targetLayer;
        private float _speed;
        private float _stopDistance;
        private bool _initialized;

        private Unit _currentTarget;
        private float _targetCheckInterval = 0.25f;
        private float _targetCheckTimer;

        public void Init(
            BattlefieldInterfaceForUnit battlefieldInterface,
            Faction targetFaction,
            float speed,
            LayerMask targetLayer,
            float stopDistance
        )
        {
            _battlefieldInterface = battlefieldInterface;
            _targetFaction = targetFaction;
            _speed = speed;
            _targetLayer = targetLayer;
            _stopDistance = stopDistance;

            _rb = GetComponent<Rigidbody2D>();
            _rb.freezeRotation = true;
            _rb.gravityScale = 0f;

            _initialized = true;
        }

        private void FixedUpdate()
        {
            if (!_initialized)
                return;

            // Update target periodically
            _targetCheckTimer -= Time.fixedDeltaTime;
            if (_targetCheckTimer <= 0f)
            {
                _currentTarget = _battlefieldInterface
                    .GetClosestUnitOfFaction(_targetFaction, transform, 100, _targetLayer);
                _targetCheckTimer = _targetCheckInterval;
            }

            if (_currentTarget == null)
                return;

            // Stop moving if close enough
            float distance = Vector2.Distance(_currentTarget.GetPosition(), transform.position);
            if (distance <= _stopDistance)
                return;

            // Move toward target
            Vector2 direction = (_currentTarget.GetPosition() - transform.position).normalized;
            Vector2 newPos = _rb.position + direction * _speed * Time.fixedDeltaTime;
            _rb.MovePosition(newPos);
        }
    }
}