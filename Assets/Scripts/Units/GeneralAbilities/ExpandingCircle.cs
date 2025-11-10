using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Units.GeneralAbilities
{
    [RequireComponent(typeof(CircleCollider2D), typeof(LineRenderer))]
    [DisallowMultipleComponent]
    public class ExpandingCircle : MonoBehaviour
    {
        [Header("Targeting")] 
        public LayerMask targetLayers = ~0;
        public bool reactToTriggers = true;

        [Header("Effect")] 
        public EffectMode effectMode = EffectMode.Push;
        public float effectStrength = 5f;
        public ForceMode2D forceMode = ForceMode2D.Impulse;

        [Header("Events")] 
        public UnityEvent<Collider2D> onTouch;

        [Header("Visualization")]
        public bool drawRing = true;
        [Min(3)] public int ringSegments = 64;
        [Min(0f)] public float ringWidth = 0.05f;
        public string ringSortingLayer = "Default";
        public int ringSortingOrder = 0;
        public Color ringColor = new Color(1f, 0.5f, 0f, 1f);

        // —— Runtime-configurable fields (set via Init)
        float _startRadius;
        float _maxRadius;
        float _expansionSpeed;
        bool  _destroyOnMax;
        bool  _stopAtMax;
        float _initialDelay;
        float _damagePerHit;

        // —— Internals
        CircleCollider2D _circle;
        LineRenderer     _ring;
        float _delayTimer;
        bool  _started;
        float _rehitCooldown = 0.1f;
        float _sweepTimer;
        readonly Dictionary<Collider2D, float> _lastHitTime = new();

        public enum EffectMode { None, Push, Pull }

        // Public accessor
        public float Radius => _circle != null ? _circle.radius : 0f;

        // ---------- Factory & Init ----------

        public struct Params
        {
            public Vector3 position;
            public Transform parent;
            public float startRadius;
            public float maxRadius;
            public float expansionSpeed;
            public float initialDelay;
            public int damagePerHit;
            public bool  destroyOnMax;
            public bool  stopAtMax;

            // Optional quick visual overrides
            public bool? drawRing;
            public int?  ringSegments;
            public float? ringWidth;
            public Color? ringColor;
            public string ringSortingLayer;
            public int?  ringSortingOrder;

            // Optional effect overrides
            public EffectMode? effectMode;
            public float? effectStrength;
            public ForceMode2D? forceMode;
            public LayerMask? targetLayers;
            public bool? reactToTriggers;
        }

        /// <summary>
        /// Spawns and configures an expanding circle with code-defined parameters.
        /// </summary>
        public static ExpandingCircle Spawn(Params p)
        {
            var go = new GameObject("ExpandingCircle");
            if (p.parent) { go.transform.SetParent(p.parent, false); go.transform.position = p.position; }
            else          { go.transform.position = p.position; }

            var circle = go.AddComponent<ExpandingCircle>();
            circle.Init(p);
            return circle;
        }

        /// <summary> Apply configuration and prepare for expansion. </summary>
        public void Init(Params p)
        {
            // Required components
            _circle = GetComponent<CircleCollider2D>();
            _ring   = GetComponent<LineRenderer>();

            // Runtime config
            _startRadius    = Mathf.Max(0f, p.startRadius);
            _maxRadius      = Mathf.Max(0f, p.maxRadius);
            _expansionSpeed = Mathf.Max(0f, p.expansionSpeed);
            _destroyOnMax   = p.destroyOnMax;
            _stopAtMax      = p.stopAtMax;
            _initialDelay   = Mathf.Max(0f, p.initialDelay);
            _damagePerHit   = Mathf.Max(0f, p.damagePerHit);

            // Optional overrides
            if (p.drawRing.HasValue)        drawRing = p.drawRing.Value;
            if (p.ringSegments.HasValue)    ringSegments = Mathf.Max(3, p.ringSegments.Value);
            if (p.ringWidth.HasValue)       ringWidth = Mathf.Max(0f, p.ringWidth.Value);
            if (p.ringColor.HasValue)       ringColor = p.ringColor.Value;
            if (!string.IsNullOrEmpty(p.ringSortingLayer)) ringSortingLayer = p.ringSortingLayer;
            if (p.ringSortingOrder.HasValue) ringSortingOrder = p.ringSortingOrder.Value;

            if (p.effectMode.HasValue)      effectMode = p.effectMode.Value;
            if (p.effectStrength.HasValue)  effectStrength = p.effectStrength.Value;
            if (p.forceMode.HasValue)       forceMode = p.forceMode.Value;
            if (p.targetLayers.HasValue)    targetLayers = p.targetLayers.Value;
            if (p.reactToTriggers.HasValue) reactToTriggers = p.reactToTriggers.Value;

            // Collider
            _circle.isTrigger = true;
            _circle.radius = Mathf.Clamp(_startRadius, 0f, _maxRadius > 0f ? _maxRadius : Mathf.Infinity);

            // Ring
            SetupRing();
            _delayTimer = _initialDelay;
            _started = (_initialDelay <= 0f);
            if (drawRing) UpdateRing();
        }

        void SetupRing()
        {
            if (!drawRing) return;

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

            if (_ring.sharedMaterial == null)
                _ring.sharedMaterial = new Material(Shader.Find("Sprites/Default"));

            _ring.startColor = ringColor;
            _ring.endColor   = ringColor;
        }

        void Update()
        {
            // Handle initial delay
            if (!_started)
            {
                _delayTimer -= Time.deltaTime;
                if (_delayTimer <= 0f) _started = true;
                else { if (drawRing) UpdateRing(); return; }
            }

            // Expand
            if (_expansionSpeed > 0f)
            {
                float newRadius = _circle.radius + _expansionSpeed * Time.deltaTime;

                if (_maxRadius > 0f && newRadius >= _maxRadius)
                {
                    newRadius = _maxRadius;
                    if (_stopAtMax) _expansionSpeed = 0f;
                    if (_destroyOnMax) { Destroy(gameObject); return; }
                }
                _circle.radius = newRadius;
            }

            // Housekeeping
            _sweepTimer += Time.deltaTime;
            if (_sweepTimer > 1f) { _sweepTimer = 0f; SweepRehitMap(); }

            if (drawRing) UpdateRing();
        }

        void UpdateRing()
        {
            if (_ring == null || ringSegments < 3) return;

            float angleStep = Mathf.PI * 2f / ringSegments;
            Vector3 center = transform.position;
            float r = _circle.radius;

            for (int i = 0; i < ringSegments; i++)
            {
                float angle = i * angleStep;
                float x = Mathf.Cos(angle) * r;
                float y = Mathf.Sin(angle) * r;
                _ring.SetPosition(i, new Vector3(center.x + x, center.y + y, center.z));
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!ShouldReactTo(other)) return;
            TryAffect(other);
        }

        bool ShouldReactTo(Collider2D other)
        {
            if (other == null || other == _circle) return false;
            if (((1 << other.gameObject.layer) & targetLayers) == 0) return false;
            if (!reactToTriggers && other.isTrigger) return false;
            return true;
        }

        void TryAffect(Collider2D other)
        {
            _lastHitTime[other] = Time.time;

            onTouch?.Invoke(other);

            var unit = other.GetComponent<Unit>();
            if (unit != null && _damagePerHit > 0f)
            {
                unit.TakeDamage((int)_damagePerHit);
            }

            switch (effectMode)
            {
                case EffectMode.Push: ApplyDirectionalForce(other, true);  break;
                case EffectMode.Pull: ApplyDirectionalForce(other, false); break;
            }
        }

        void ApplyDirectionalForce(Collider2D other, bool push)
        {
            if (other.attachedRigidbody != null && other.attachedRigidbody.bodyType != RigidbodyType2D.Static)
            {
                Vector2 dir = ((Vector2)other.bounds.center - (Vector2)transform.position);
                if (dir.sqrMagnitude < 0.0001f) dir = Random.insideUnitCircle.normalized;
                dir = dir.normalized * (push ? 1f : -1f);
                other.attachedRigidbody.AddForce(dir * effectStrength, forceMode);
            }
            else
            {
                Vector2 dir = ((Vector2)other.bounds.center - (Vector2)transform.position).normalized;
                if (!push) dir = -dir;
                other.transform.position += (Vector3)(dir * effectStrength * Time.deltaTime);
            }
        }

        void SweepRehitMap()
        {
            if (_lastHitTime.Count == 0) return;
            float threshold = Time.time - Mathf.Max(0.5f, _rehitCooldown * 5f);
            var toRemove = new List<Collider2D>();
            foreach (var kvp in _lastHitTime)
                if (kvp.Value < threshold) toRemove.Add(kvp.Key);
            foreach (var c in toRemove) _lastHitTime.Remove(c);
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (_circle == null) _circle = GetComponent<CircleCollider2D>();
            float r = _circle != null ? _circle.radius : _startRadius;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, r);

            if (_maxRadius > 0f)
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.25f);
                Gizmos.DrawWireSphere(transform.position, _maxRadius);
            }
        }
#endif
    }
}
