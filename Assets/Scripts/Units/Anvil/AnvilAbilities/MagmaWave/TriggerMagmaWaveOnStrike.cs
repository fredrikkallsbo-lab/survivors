using System;
using System.Collections.Generic;
using Units.Abilities;
using Units.Anvil.AnvilAbilities.MagmaWave.Upgrade;
using Units.Resources;
using UnityEngine;

namespace Units.Anvil.AnvilAbilities
{
    public class TriggerMagmaWaveOnStrike: ITrigger
    {
        private int _lastTriggeredOnStrikeNr = 0;
        private int _triggerFrequency;
        public readonly ResourceId ResourceId = ResourceId.AnvilStrike;
        
        private readonly IEventBus _eventBus;
        IDisposable _subscription;

        private AnvilSpellcaster _anvilSpellcaster;

        private float _maxRadius;
        private float _speed;
        private int _damage;
        
        public TriggerMagmaWaveOnStrike(IEventBus eventBus, AnvilSpellcaster anvilSpellcaster, int triggerFrequency)
        {
            _triggerFrequency = triggerFrequency;
            _eventBus = eventBus;
            _anvilSpellcaster = anvilSpellcaster;
        }

        public void UpdateValues(float maxRadius, float maxSpeed, int damage)
        {
            _maxRadius = maxRadius;
            _speed = maxSpeed;
            _damage = damage;
        }
        
        public void Enable()
        {
            _subscription = _eventBus.Subscribe<ResourceChanged>(e =>
            {
                if (e.ResourceId == ResourceId)
                {
                    if (e.NewValue >= _lastTriggeredOnStrikeNr + _triggerFrequency)
                    {
                        _lastTriggeredOnStrikeNr = e.NewValue;
                        Trigger();
                    } 
                }
            });
        }

        public void Disable()
        {
            _subscription.Dispose();
        }

        private void Trigger()
        {
            _anvilSpellcaster.CastMagmaWave(_maxRadius, _speed, _damage);
        }
    }
}