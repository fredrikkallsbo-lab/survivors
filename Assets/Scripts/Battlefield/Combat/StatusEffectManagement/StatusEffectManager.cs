using System;
using System.Collections.Generic;
using Battlefield.Combat.StatusEffectManagement;
using Units;

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
    }
}