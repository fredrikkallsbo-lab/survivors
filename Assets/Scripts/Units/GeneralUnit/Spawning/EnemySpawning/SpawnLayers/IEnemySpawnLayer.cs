using System;

namespace Battlefield
{
    public interface IEnemySpawnLayer
    {
        IEnemySpawnLayer Configure(
            float startDelaySeconds, //= 0f,
            float baseIntervalSeconds, //= 5f,
            float minIntervalSeconds, //= 2f,
            int intensityForMaxInterval, //= 0,
            int intensityForMinInterval, //= 100,
            Action spawnCallback);

        void Start(Scheduler scheduler, IntensityProvider intensityProvider);
        
        void Stop();
    }
}