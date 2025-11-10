using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;

namespace Units.Anvil.AnvilAbilities.Chains
{
    
    
    public class AbilityEffectChains : IAbilityEffect
    {
        
        private TriggerManager _triggerManager;
        private AnvilSpellcaster _anvilSpellcaster;
        private IEventBus _eventBus;
        private ITrigger _trigger;

        public AbilityEffectChains(AnvilSpellcaster anvilSpellcaster, TriggerManager triggerManager, IEventBus eventBus)
        {
            _triggerManager = triggerManager;
            _anvilSpellcaster = anvilSpellcaster;
            _eventBus = eventBus;
        }

        public void Init(AbilityModifier abilityModifier)
        {
            _trigger = new TriggerChainOnStrike(_anvilSpellcaster, _eventBus);
            _triggerManager.Add(_trigger);
        }

        public void RefreshAbilityModifier(AbilityModifier abilityModifier)
        {
        }

        public void ManualOnDisable()
        {
            _triggerManager.Remove(_trigger);
        }
    }
}