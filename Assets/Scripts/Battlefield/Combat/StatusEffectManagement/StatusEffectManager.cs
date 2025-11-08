using System;
using System.Collections.Generic;
using Battlefield.Combat.StatusEffectManagement;
using Units;
using Units.Anvil.AnvilAbilities.Chains;

namespace Battlefield.GameMechanics.Combat.BuffManagement
{
    public class StatusEffectManager
    {
        private List<IStatusEffect> _statusEffects;
        private Unit _affectedUnit;
        
        public StatusEffectManager(Unit unit)
        {
            _statusEffects = new List<IStatusEffect>();
            _affectedUnit = unit;
        }

        public void AddStatusEffect(IStatusEffect statusEffect)
        {
            _statusEffects.Add(statusEffect);
        }

        public bool HasStatusEffect(StatusEffectId statusEffectId)
        {
            foreach (IStatusEffect statusEffect in _statusEffects)
            {
                if (statusEffect.GetStatusEffectId() == statusEffectId)
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveStatusEffect(StatusEffectChain statusEffectChain)
        {
            _statusEffects.Remove(statusEffectChain);
            statusEffectChain.RemoveStatusEffect();
        }

        public void UnitDeath()
        {
            foreach (IStatusEffect statusEffect in _statusEffects)
            {
                statusEffect.RemoveStatusEffect();
            }
        }
    }
}