using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Units.Abilities
{
    [RequireComponent(typeof(CircleCollider2D), typeof(LineRenderer))]
    public class ExpandingCircle : MonoBehaviour
    {
        [Header("Expansion")] [Min(0f)] public float startRadius = 0.1f;
        [Min(0f)] public float maxRadius = 5f;
        [Min(0f)] public float expansionSpeed = 1f; // units of collider radius per second
        public bool destroyOnMax = true; // destroy GameObject when reaching max
        public bool stopAtMax = true; // stop expanding at max

        [Header("Targeting")] public LayerMask targetLayers = ~0; // which layers to react to
        public bool reactToTriggers = true; // should we react to other triggers?

        [Header("Effect")] public EffectMode effectMode = EffectMode.Push;
        public float effectStrength = 5f; // magnitude for push/pull
        public ForceMode2D forceMode = ForceMode2D.Impulse;

        [Header("Events")] public UnityEvent<Collider2D> onTouch; // invoked when we touch a collider

        // Optional: cooldown so we don't spam the same target every frame if using Stay
        [Min(0f)] public float rehitCooldown = 0.1f;

        
        [Header("Visualization")]
        public bool drawRing = true;
        [Min(3)] public int ringSegments = 64;
        [Min(0f)] public float ringWidth = 0.05f;
        public string ringSortingLayer = "Default";
        public int ringSortingOrder = 0;

        private LineRenderer _ring;
        
        
        // Public accessor
        public float Radius => _circle.radius;

        // Interface hook: any component implementing this will be called
        // on touch so you can do custom effects without modifying this class.
        public interface IExpandingCircleAffectable
        {
            void OnTouchedByCircle(ExpandingCircle circle, Collider2D selfCollider);
        }

        public enum EffectMode
        {
            None,
            Push, // push away from circle center
            Pull // pull toward circle center
        }

        private CircleCollider2D _circle;
        private float _timeSinceRehitSweep;
        private readonly Dictionary<Collider2D, float> _lastHitTime = new();

        private void Awake()
        {
            destroyOnMax = true;
            _circle = GetComponent<CircleCollider2D>();
            _circle.isTrigger = true;
            _circle.radius = Mathf.Clamp(startRadius, 0f, maxRadius > 0f ? maxRadius : Mathf.Infinity);
            
            // --- Ring setup ---
            _ring = GetComponent<LineRenderer>();
            if (drawRing)
            {
                if (_ring == null) _ring = gameObject.AddComponent<LineRenderer>();
                _ring.useWorldSpace = true;
                _ring.loop = true;
                _ring.positionCount = ringSegments;
                _ring.startWidth = ringWidth;
                _ring.endWidth = ringWidth;
                _ring.numCornerVertices = 2;
                _ring.numCapVertices = 2;
                _ring.sortingLayerName = ringSortingLayer;
                _ring.sortingOrder = ringSortingOrder;

                // Basic material so it shows up; you can swap in your own in the Inspector
                if (_ring.sharedMaterial == null)
                    _ring.sharedMaterial = new Material(Shader.Find("Sprites/Default"));

                // Orange color
                _ring.startColor = new Color(1f, 0.5f, 0f, 1f);
                _ring.endColor   = new Color(1f, 0.5f, 0f, 1f);

                UpdateRing(); // draw initial
            }
        }

        private void UpdateRing()
        {
            if (_ring == null || ringSegments < 3) return;

            float angleStep = Mathf.PI * 2f / ringSegments;
            Vector3 center = transform.position;
            float r = _circle.radius;

            // Place points around the circumference
            for (int i = 0; i < ringSegments; i++)
            {
                float angle = i * angleStep;
                float x = Mathf.Cos(angle) * r;
                float y = Mathf.Sin(angle) * r;
                _ring.SetPosition(i, new Vector3(center.x + x, center.y + y, center.z));
            }
        }
        
        private void Update()
        {
            // Expand radius
            if (expansionSpeed > 0f)
            {
                var newRadius = _circle.radius + expansionSpeed * Time.deltaTime;

                if (maxRadius > 0f && newRadius >= maxRadius)
                {
                    newRadius = maxRadius;

                    if (stopAtMax) expansionSpeed = 0f;
                    if (destroyOnMax)
                    {
                        // Invoke one last overlap pass before destroy (optional)
                        // Physics engine will have already fired events.
                        Destroy(gameObject);
                    }
                }

                _circle.radius = newRadius;
            }

            // Advance time for re-hit bookkeeping and clean up stale entries occasionally
            _timeSinceRehitSweep += Time.deltaTime;
            if (_timeSinceRehitSweep > 1f)
            {
                _timeSinceRehitSweep = 0f;
                SweepRehitMap();
            }
            UpdateRing();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("OnTriggerEnter2D: " + other.name);
            if (!ShouldReactTo(other)) return;
            TryAffect(other);
        }

        /*private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log("OnTriggerStay2D");
            if (!ShouldReactTo(other)) return;

            // Respect re-hit cooldown to avoid excessive calls/forces
            if (rehitCooldown <= 0f)
            {
                TryAffect(other);
                return;
            }

            if (!_lastHitTime.TryGetValue(other, out var last) || (Time.time - last) >= rehitCooldown)
            {
                TryAffect(other);
            }
        }*/

        private bool ShouldReactTo(Collider2D other)
        {
            if (other == null || other == _circle) return false;

            // Layer mask
            if (((1 << other.gameObject.layer) & targetLayers) == 0) return false;

            // Trigger policy
            if (!reactToTriggers && other.isTrigger) return false;

            // Ignore self / same root if desired (optional; commented out)
            // if (other.transform.root == transform.root) return false;

            return true;
        }

        private void TryAffect(Collider2D other)
        {
            _lastHitTime[other] = Time.time;

            /// UnityEvent for designer-friendly hooks
            //onTouch?.Invoke(other);

            // Interface hook for code-based custom effects
            var custom = other.GetComponent<IExpandingCircleAffectable>();
            custom?.OnTouchedByCircle(this, other);

            Unit otherUnit = other.GetComponent<Unit>();
            otherUnit.TakeDamage(5);
            
            // Built-in simple effects
            switch (effectMode)
            {
                case EffectMode.Push:
                    ApplyDirectionalForce(other, push: true);
                    break;
                case EffectMode.Pull:
                    ApplyDirectionalForce(other, push: false);
                    break;
                case EffectMode.None:
                default:
                    break;
            }
        }

        private void ApplyDirectionalForce(Collider2D other, bool push)
        {
            // Prefer Rigidbody2D for proper physics
            if (other.attachedRigidbody != null && other.attachedRigidbody.bodyType != RigidbodyType2D.Static)
            {
                Vector2 dir = ((Vector2)other.bounds.center - (Vector2)transform.position);
                if (dir.sqrMagnitude < 0.0001f) dir = Random.insideUnitCircle.normalized; // avoid NaN
                dir = dir.normalized * (push ? 1f : -1f);

                other.attachedRigidbody.AddForce(dir * effectStrength, forceMode);
            }
            else
            {
                // Fallback: nudge transform directly (non-physical)
                Vector2 dir = ((Vector2)other.bounds.center - (Vector2)transform.position).normalized;
                if (!push) dir = -dir;
                other.transform.position += (Vector3)(dir * effectStrength * Time.deltaTime);
            }
        }

        private void SweepRehitMap()
        {
            // Remove entries we haven’t touched for a while (5x cooldown window)
            if (_lastHitTime.Count == 0) return;
            float threshold = Time.time - Mathf.Max(0.5f, rehitCooldown * 5f);

            // Avoid allocation by reusing a list if you like; fine for now.
            var toRemove = new List<Collider2D>();
            foreach (var kvp in _lastHitTime)
            {
                if (kvp.Value < threshold) toRemove.Add(kvp.Key);
            }

            foreach (var c in toRemove) _lastHitTime.Remove(c);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Draw current (or intended) radius
            float r = _circle != null ? _circle.radius : startRadius;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, r);

            if (maxRadius > 0f)
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.25f);
                Gizmos.DrawWireSphere(transform.position, maxRadius);
            }
        }
#endif
    }
}