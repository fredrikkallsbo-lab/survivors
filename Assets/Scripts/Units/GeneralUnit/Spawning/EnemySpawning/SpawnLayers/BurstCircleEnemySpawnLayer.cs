using System;
using UnityEngine;

namespace Battlefield
{
    public class BurstCircleEnemySpawnLayer : IEnemySpawnLayer
    {
        // --- Config ---
        private float _startDelaySeconds = 0f;
        private float _intervalSeconds   = 30f;
        private int   _burstCount        = 5;
        private float _burstRadius       = 1.0f;     // desired (tight) radius
        private float _minSeparation     = 1.0f;     // NEW: desired minimum spacing between neighbors
        private float _radialJitterMax   = 0.15f;    // NEW: small outward jitter for visual variety

        // (parity fields omitted for brevity)
        private int _intensityForMaxInterval = 0;
        private int _intensityForMinInterval = 100;

        // --- Runtime deps ---
        private Scheduler _scheduler;
        private IntensityProvider _intensityProvider;
        private Action _spawnCallback;

        private Action<Vector3> _spawnAtCallback;
        private Func<Vector3> _centerProvider;
        private Vector3 _staticCenter = Vector3.zero;

        // --- State ---
        private Action _cancelToken;
        private bool _running;

        public IEnemySpawnLayer Configure(
            float startDelaySeconds,
            float baseIntervalSeconds,
            float minIntervalSeconds,
            int intensityForMaxInterval,
            int intensityForMinInterval,
            Action spawnCallback)
        {
            _startDelaySeconds = Mathf.Max(0f, startDelaySeconds);
            _intervalSeconds   = Mathf.Max(0.1f, baseIntervalSeconds);
            _intensityForMaxInterval = intensityForMaxInterval;
            _intensityForMinInterval = intensityForMinInterval;
            _spawnCallback = spawnCallback;
            return this;
        }

        public BurstCircleEnemySpawnLayer WithBurst(int count = 5, float radius = 1.0f)
        {
            _burstCount  = Mathf.Max(1, count);
            _burstRadius = Mathf.Max(0.01f, radius);
            return this;
        }

        // NEW: configure minimum spacing
        public BurstCircleEnemySpawnLayer WithMinSeparation(float minSeparation, float radialJitterMax = 0.15f)
        {
            _minSeparation   = Mathf.Max(0f, minSeparation);
            _radialJitterMax = Mathf.Max(0f, radialJitterMax);
            return this;
        }

        public BurstCircleEnemySpawnLayer WithSpawnAt(Action<Vector3> spawnAt)
        {
            _spawnAtCallback = spawnAt;
            return this;
        }

        public BurstCircleEnemySpawnLayer WithCenter(Func<Vector3> centerProvider)
        {
            _centerProvider = centerProvider;
            return this;
        }

        public BurstCircleEnemySpawnLayer WithCenter(Vector3 center)
        {
            _staticCenter = center;
            _centerProvider = null;
            return this;
        }

        public void Start(Scheduler scheduler, IntensityProvider intensityProvider)
        {
            Stop();
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _intensityProvider = intensityProvider;
            if (_spawnCallback == null && _spawnAtCallback == null)
                throw new ArgumentNullException(nameof(_spawnCallback), "Provide either spawnCallback in Configure or a SpawnAt callback via WithSpawnAt().");
            _running = true;
            ScheduleNext(_startDelaySeconds);
        }

        public void Stop()
        {
            _running = false;
            _cancelToken?.Invoke();
            _cancelToken = null;
        }

        private void Tick()
        {
            if (!_running) return;

            if (_spawnAtCallback != null)
            {
                var center = _centerProvider != null ? _centerProvider() : _staticCenter;

                // --- compute effective radius to satisfy min separation ---
                float requiredR = 0f;
                if (_burstCount > 1 && _minSeparation > 0f)
                {
                    float sinTerm = Mathf.Sin(Mathf.PI / _burstCount);
                    if (sinTerm > 1e-5f)
                        requiredR = _minSeparation / (2f * sinTerm);
                }
                float R = Mathf.Max(_burstRadius, requiredR);

                // place N evenly; add a touch of outward jitter for variety
                for (int i = 0; i < _burstCount; i++)
                {
                    float angle = (Mathf.PI * 2f * i) / _burstCount;
                    var dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

                    float jitter = (_radialJitterMax > 0f) ? UnityEngine.Random.Range(0f, _radialJitterMax) : 0f;
                    var pos = center + dir * (R + jitter);

                    _spawnAtCallback(pos);
                }
            }
            else
            {
                // Fallback: simple spawns (can still overlap if your default placement overlaps)
                for (int i = 0; i < _burstCount; i++)
                    _spawnCallback?.Invoke();
            }

            ScheduleNext(_intervalSeconds);
        }

        private void ScheduleNext(float delaySeconds)
        {
            _cancelToken?.Invoke();
            var delay = Math.Max(0.01f, delaySeconds);

            Action localCancel = null;
            localCancel = _scheduler.Every(delay, () =>
            {
                localCancel?.Invoke();
                if (_running) Tick();
            });

            _cancelToken = localCancel;
        }
    }
}