using System;
using System.Collections.Generic;
using Units.Resources;
using UnityEngine;

namespace Units.Anvil.AnvilAbilities.Chains
{
    public class TriggerChainOnStrike : ITrigger
    {

        private int _lastTriggeredOnStrikeNr = 0;
        private int _triggerFrequency = 3;
        private AnvilSpellcaster _anvilSpellcaster;
        
        public readonly ResourceId ResourceId = ResourceId.AnvilStrike;
        private readonly IEventBus _eventBus;
        IDisposable _subscription;


        public TriggerChainOnStrike(AnvilSpellcaster anvilSpellcaster, IEventBus eventBus)
        {
            _anvilSpellcaster = anvilSpellcaster;
            _eventBus = eventBus;
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

        private void Trigger()
        {
            _anvilSpellcaster.CastChains();
        }

        public void Disable()
        {
            _subscription.Dispose();
        }
    }
}