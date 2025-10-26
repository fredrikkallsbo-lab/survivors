using System;
using System.Collections.Generic;
using Units.Abilities;
using Units.Resources;
using UnityEngine;

namespace Units.Anvil.AnvilAbilities
{
    public class TriggerMagmaWaveOnStrike: ITrigger
    {
        private int _lastTriggeredOnStrikeNr = 0;
        private int _triggerFrequency = 10;
        public readonly ResourceId ResourceId = ResourceId.AnvilStrike;
        
        private readonly IEventBus _eventBus;
        IDisposable _subscription;

        private AnvilSpellcaster _anvilSpellcaster;
        
        public TriggerMagmaWaveOnStrike(IEventBus eventBus, AnvilSpellcaster anvilSpellcaster)
        {
            _eventBus = eventBus;
            _anvilSpellcaster = anvilSpellcaster;
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
            _anvilSpellcaster.CastMagmaWave();
        }
    }
}