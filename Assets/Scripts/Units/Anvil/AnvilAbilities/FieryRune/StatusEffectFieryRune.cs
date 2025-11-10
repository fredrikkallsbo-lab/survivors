using System.Collections.Generic;
using Battlefield.Combat.StatusEffectManagement;
using Battlefield.GameMechanics.Combat.BuffManagement;
using UnityEngine;

namespace Units.Anvil.AnvilAbilities
{
    public class StatusEffectFieryRune : IStatusEffect
    {
        
        private Unit _targetUnit;
        public readonly StatusEffectId StatusEffectId = StatusEffectId.FieryRune;
        private IEventBus _eventBus;

        private FieryRuneVisual _fieryRuneVisual;
        private TriggerFieryRuneExplosionOnDeath _triggerFieryRuneExplosion;

        public StatusEffectFieryRune(Unit targetUnit, IEventBus eventBus)
        {
            _targetUnit = targetUnit;
            _eventBus = eventBus;
        }

        public void ApplyStatusEffect(Unit targetUnit)
        {
            _targetUnit = targetUnit;
            _fieryRuneVisual = FieryRuneVisual.CreateFromResources(
                _targetUnit.gameObject,
                "Sprites/Random/FieryRune",
                sortingOrderOffset: 10,
                scale: 1.3f
            );
            _triggerFieryRuneExplosion = new TriggerFieryRuneExplosionOnDeath(_eventBus, _targetUnit);
            _targetUnit.AddTrigger(_triggerFieryRuneExplosion);
        }

        public StatusEffectId GetStatusEffectId()
        {
            return StatusEffectId;
        }

        public void RemoveStatusEffect()
        {
            _targetUnit.RemoveTrigger(_triggerFieryRuneExplosion);
            Object.Destroy(_fieryRuneVisual);
        }
    }
}