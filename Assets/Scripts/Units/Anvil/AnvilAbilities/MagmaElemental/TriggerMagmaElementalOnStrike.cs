using System;
using System.Collections.Generic;
using Units.Resources;

namespace Units.Anvil.AnvilAbilities.MagmaElemental
{
    public class TriggerMagmaElementalOnStrike :ITrigger
    {
        private int _lastTriggeredOnStrikeNr = 0;
        private int _triggerFrequency = 5;
        public readonly ResourceId ResourceId = ResourceId.AnvilStrike;
        
        private readonly IEventBus _eventBus;
        IDisposable _subscription;
        
        private AnvilSpellcaster _anvilSpellcaster;

        public TriggerMagmaElementalOnStrike(IEventBus eventBus, AnvilSpellcaster anvilSpellcaster)
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
            _anvilSpellcaster.CastMagmaElemental();
        }
    }
}