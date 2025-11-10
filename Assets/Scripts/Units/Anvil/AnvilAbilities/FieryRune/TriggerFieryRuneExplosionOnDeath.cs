using System;
using System.Collections.Generic;
using Battlefield.GameEvents;
using Units.GeneralAbilities;
using UnityEngine;

namespace Units.Anvil.AnvilAbilities
{
    public class TriggerFieryRuneExplosionOnDeath : ITrigger
    {
        private IEventBus _eventBus;
        private Unit _affectedUnit;

        private IDisposable _unitEnemySub;

        
        
        public TriggerFieryRuneExplosionOnDeath(IEventBus eventBus, Unit affectedUnit)
        {
            _eventBus = eventBus;
            _affectedUnit = affectedUnit;
        }

        public void Enable()
        {
            _unitEnemySub = _eventBus.Subscribe<EnemyUnitDeathEvent>(e =>
            {
                var deadUnit = e.GetUnit();
                if (_affectedUnit == deadUnit)
                {
                    TriggerOnEnemyDeath(deadUnit);
                }
            });
        }

        private void TriggerOnEnemyDeath(Unit unit)
        {
            var p = new ExpandingCircle.Params
            {
                position       = unit.GetPosition(),
                parent         = null,               // or a transform to parent under
                startRadius    = 0.05f,
                maxRadius      = 1f,
                expansionSpeed = 0.4f,               // units/sec
                initialDelay   = 0.5f,               // wait before expanding
                damagePerHit   = 40,
                destroyOnMax   = true,
                stopAtMax      = true,

                // Optional overrides
                drawRing       = true,
                ringSegments   = 64,
                ringWidth      = 0.06f,
                ringColor      = new Color(1f, 0.4f, 0.1f, 1f),
                ringSortingLayer = "Effects",
                ringSortingOrder = 10,
                effectMode     = ExpandingCircle.EffectMode.Push,
                effectStrength = 5f,
                forceMode      = ForceMode2D.Impulse,
                targetLayers   = LayerMask.GetMask("Enemy"),
                reactToTriggers = true
            };
            var circle = ExpandingCircle.Spawn(p);
        }

        public void Disable()
        {
            _unitEnemySub.Dispose();
        }
    }
}