using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;
using Units.Resources;

namespace Units.Anvil.AnvilAbilities.AnvilStrike
{
    public class AbilityAnvilStrike : IAbility
    {
        
        private static readonly AbilityId AbilityId = AbilityId.AnvilStrike;

        private AbilityEffectAnvilStrike _abilityEffectAnvilStrike;

        public AbilityAnvilStrike(
            Scheduler scheduler, 
            UnitResourceInterface unitResourceInterface,
            IEventBus eventBus)
        {
            _abilityEffectAnvilStrike = new AbilityEffectAnvilStrike(scheduler, unitResourceInterface, eventBus);
        }

        public void Init(AbilityModifier abilityModifier)
        {
            _abilityEffectAnvilStrike.Init(abilityModifier);
        }

        public void RefreshAbilityModifierSet(AbilityModifier abilityModifierSet)
        {
            return;
        }

        public void ManualOnDisable()
        {
            _abilityEffectAnvilStrike.ManualOnDisable();
        }

        public AbilityId GetAbilityId()
        {
            return AbilityId;
        }

        public List<IAbilityUpgrade> GetUpgrades(int numberOfUpgrades)
        {
            throw new System.NotImplementedException();
        }
    }
}