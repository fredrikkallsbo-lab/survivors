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
        private FieryRuneVisual _fieryRuneVisual;

        
        public void ApplyStatusEffect(Unit unit)
        {
            _unit = unit;
            _fieryRuneVisual = FieryRuneVisual.CreateFromResources(
                _unit.gameObject,
                "Sprites/Random/FieryRune",
                sortingOrderOffset: 10,
                scale: 1.3f
            );
            Debug.Log("Here it should add on death trigger");
        }

        public StatusEffectId GetStatusEffectId()
        {
            return StatusEffectId;
        }

        public void RemoveStatusEffect()
        {
            _unit.RemoveTrigger(_triggerFieryRuneOnStrike);
            Object.Destroy(_fieryRuneVisual);
        }
    }
}