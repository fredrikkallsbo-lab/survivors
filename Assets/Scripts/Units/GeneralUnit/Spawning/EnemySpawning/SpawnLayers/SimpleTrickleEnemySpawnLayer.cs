using System;
using UnityEngine;

namespace Battlefield
{
    public class SimpleTrickleEnemySpawnLayer : IEnemySpawnLayer
    {
        // Config
        private float _startDelaySeconds = 0f;
        private float _baseIntervalSeconds = 5f;
        private float _minIntervalSeconds  = 2f;

        private int _intensityForMaxInterval = 0;
        private int _intensityForMinInterval = 100;

        // Runtime deps
        private Scheduler _scheduler;
        private IntensityProvider _intensityProvider;
        private Action _spawnCallback;

        // State
        private Action _cancelToken;
        private bool _running;

        public IEnemySpawnLayer Configure(
            float startDelaySeconds, //= 0f,
            float baseIntervalSeconds, //= 5f,
            float minIntervalSeconds, //= 2f,
            int intensityForMaxInterval, //= 0,
            int intensityForMinInterval, //= 100,
            Action spawnCallback) //= null)
        {
            _startDelaySeconds = Mathf.Max(0f, startDelaySeconds);
            _baseIntervalSeconds = Mathf.Max(0.01f, baseIntervalSeconds);
            _minIntervalSeconds  = Mathf.Max(0.01f, minIntervalSeconds);
            _intensityForMaxInterval = intensityForMaxInterval;
            _intensityForMinInterval = intensityForMinInterval;
            _spawnCallback = spawnCallback;
            return this;
        }

        

        public void Start(Scheduler scheduler, IntensityProvider intensityProvider)
        {
            Stop();

            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _intensityProvider = intensityProvider ?? throw new ArgumentNullException(nameof(intensityProvider));
            _spawnCallback = _spawnCallback ?? throw new ArgumentNullException(nameof(_spawnCallback));
            _running = true;

            ScheduleNext(_startDelaySeconds);
        }

        public void Stop()
        {
            _running = false;
            _cancelToken?.Invoke();
            _cancelToken = null;
        }

        // --- internals ---

        private void Tick()
        {
            if (!_running) return;

            _spawnCallback?.Invoke();

            var nextInterval = ComputeIntervalSeconds(_intensityProvider.GetIntensity());
            ScheduleNext(nextInterval);
        }

        private void ScheduleNext(float delaySeconds)
        {
            _cancelToken?.Invoke();
            var delay = Math.Max(0.01f, delaySeconds);

            Action localCancel = null;
            localCancel = _scheduler.Every(delay, () =>
            {
                localCancel?.Invoke();    // make it one-shot
                if (_running) Tick();
            });

            _cancelToken = localCancel;
        }

        private float ComputeIntervalSeconds(int intensity)
        {
            float t;
            if (_intensityForMinInterval == _intensityForMaxInterval)
                t = intensity >= _intensityForMinInterval ? 1f : 0f;
            else
                t = Mathf.InverseLerp(_intensityForMaxInterval, _intensityForMinInterval, intensity);

            var interval = Mathf.Lerp(_baseIntervalSeconds, _minIntervalSeconds, Mathf.Clamp01(t));
            return Mathf.Max(_minIntervalSeconds, interval);
        }
    }
}