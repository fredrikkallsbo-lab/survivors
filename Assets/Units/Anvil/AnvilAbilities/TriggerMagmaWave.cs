using System;
using System.Collections.Generic;
using Units.Abilities;
using Units.Resources;
using UnityEngine;

namespace Units.Anvil.AnvilAbilities
{
    public class TriggerMagmaWave: ITrigger
    {
        private int _lastTriggeredOnStrikeNr = 0;
        private int _triggerFrequency = 5;
        public readonly ResourceId ResourceId = ResourceId.AnvilStrike;
        
        private readonly IEventBus _eventBus;
        IDisposable _subscription;

        private AnvilSpellcaster _anvilSpellcaster;
        
        public TriggerMagmaWave(IEventBus eventBus, AnvilSpellcaster anvilSpellcaster)
        {
            _eventBus = eventBus;
            _anvilSpellcaster = anvilSpellcaster;
        }

        public void Enable()
        {
            _subscription = _eventBus.Subscribe<ResourceChanged>(e =>
            {
                Debug.Log("Enabling magma strike");
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
            Debug.Log("Trigger Magma Wave");
            _anvilSpellcaster.CastMagmaWave();
        }
    }
}