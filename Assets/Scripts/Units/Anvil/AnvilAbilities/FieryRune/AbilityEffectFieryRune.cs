using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;

namespace Units.Anvil.AnvilAbilities
{
    public class AbilityEffectFieryRune : IAbilityEffect
    {
        
        
        private TriggerManager _triggerManager;
        private AnvilSpellcaster _anvilSpellcaster;
        private IEventBus _eventBus;
        private ITrigger _trigger;

        public AbilityEffectFieryRune(AnvilSpellcaster anvilSpellcaster, TriggerManager triggerManager, IEventBus eventBus)
        {

            _anvilSpellcaster = anvilSpellcaster;
            _triggerManager = triggerManager;
            _eventBus = eventBus;
        }

        public void Init(AbilityModifier abilityModifier)
        {
            _trigger = new TriggerFieryRuneOnStrike(_anvilSpellcaster, _eventBus);
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