using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;

namespace Units.Anvil.AnvilAbilities
{
    public class AbilityEffectMagmaWave : IAbilityEffect
    {
        private TriggerMagmaWave _triggerMagmaWave;
        private TriggerManager _triggerManager;
        private IEventBus _eventBus;
        
        public AbilityEffectMagmaWave(TriggerManager triggerManager, IEventBus eventBus, AnvilSpellcaster spellcaster)
        {
            _triggerManager = triggerManager;
            _eventBus = eventBus;
            _triggerMagmaWave = new TriggerMagmaWave(_eventBus,  spellcaster);
        }

        public void Init(AbilityModifierSet abilityModifierSet)
        {
            _triggerManager.Add(_triggerMagmaWave);
        }

        public void RefreshAbilityModifierSet(AbilityModifierSet abilityModifierSet)
        {
            return;
        }

        public void ManualOnDisable()
        {
            _triggerManager.Remove(_triggerMagmaWave);
        }
    }
}