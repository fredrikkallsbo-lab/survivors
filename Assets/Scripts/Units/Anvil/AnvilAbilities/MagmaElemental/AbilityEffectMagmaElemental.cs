using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;

namespace Units.Anvil.AnvilAbilities.MagmaElemental
{
    public class AbilityEffectMagmaElemental : IAbilityEffect
    {
        private TriggerMagmaElementalOnStrike _triggerMagmaElementalOnStrike;
        private TriggerManager _triggerManager;
        private IEventBus _eventBus;
        private AnvilSpellcaster _anvilSpellcaster;


        public AbilityEffectMagmaElemental(AnvilSpellcaster anvilSpellcaster, TriggerManager triggerManager, IEventBus eventBus)
        {
            _anvilSpellcaster = anvilSpellcaster;
            _triggerManager = triggerManager;
            _eventBus = eventBus;
        }

        public void Init(AbilityModifier abilityModifier)
        {
            _triggerMagmaElementalOnStrike = new TriggerMagmaElementalOnStrike(_eventBus, _anvilSpellcaster);
            _triggerManager.Add(_triggerMagmaElementalOnStrike);
        }

        public void RefreshAbilityModifier(AbilityModifier abilityModifier)
        {
            return;
        }

        public void ManualOnDisable()
        {
            _triggerManager.Remove(_triggerMagmaElementalOnStrike);
        }
    }
}