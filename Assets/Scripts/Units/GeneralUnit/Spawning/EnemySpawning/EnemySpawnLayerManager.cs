using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battlefield
{
    public class EnemySpawnLayerManager
    {
        private readonly List<IEnemySpawnLayer> _layers = new();

        public void AddLayer(IEnemySpawnLayer layer)
        {
            if (layer != null) _layers.Add(layer);
        }

        public void ClearLayers() => _layers.Clear();

        public void StartAll(Scheduler scheduler, IntensityProvider intensityProvider)
        {
            foreach (var layer in _layers)
                layer.Start(scheduler, intensityProvider);
        }

        public void StopAll()
        {
            foreach (var layer in _layers)
                layer.Stop();
        }
    }
}