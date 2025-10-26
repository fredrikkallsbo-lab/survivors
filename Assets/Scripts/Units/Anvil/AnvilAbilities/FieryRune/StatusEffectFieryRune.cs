using System.Collections.Generic;
using Battlefield.Combat.StatusEffectManagement;
using Battlefield.GameMechanics.Combat.BuffManagement;
using UnityEngine;

namespace Units.Anvil.AnvilAbilities
{
    public class StatusEffectFieryRune : IStatusEffect
    {

        private TriggerFieryRuneOnStrike _triggerFieryRuneOnStrike;
        private Unit _unit;
        public readonly StatusEffectId StatusEffectId = StatusEffectId.FieryRune;
        

        public void ApplyStatusEffect(Unit unit)
        {
            _unit = unit;
            FieryRuneVisual.CreateFromResources(
                _unit.gameObject,
                "Sprites/Random/FieryRune",
                sortingOrderOffset: 10,
                scale: 1f
            );
            _unit.AddTrigger(new StatusEffectFieryRune());
            Debug.Log("Here it should add on death trigger");
        }

        public StatusEffectId GetStatusEffectId()
        {
            return StatusEffectId;
        }

        public void RemoveStatusEffect()
        {
            _unit.RemoveTrigger(_triggerFieryRuneOnStrike);
        }
    }
}