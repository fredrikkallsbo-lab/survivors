using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;

namespace Units.Anvil.AnvilAbilities.MagmaWave
{
    public class AbilityEffectMagmaWave : IAbilityEffect
    {
        private TriggerMagmaWaveOnStrike _triggerMagmaWaveOnStrike;
        private TriggerManager _triggerManager;
        private IEventBus _eventBus;
        
        public AbilityEffectMagmaWave(TriggerManager triggerManager, IEventBus eventBus, AnvilSpellcaster spellcaster)
        {
            _triggerManager = triggerManager;
            _eventBus = eventBus;
            _triggerMagmaWaveOnStrike = new TriggerMagmaWaveOnStrike(_eventBus,  spellcaster);
        }

        public void Init(AbilityModifier abilityModifier)
        {
            _triggerManager.Add(_triggerMagmaWaveOnStrike);
        }

        public void RefreshAbilityModifier(AbilityModifier abilityModifier)
        {
            return;
        }

        public void ManualOnDisable()
        {
            _triggerManager.Remove(_triggerMagmaWaveOnStrike);
        }
    }
}