using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Anvil.AnvilResources;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;
using Units.Resources;
using UnityEngine;

namespace System.Collections.Generic
{
    public class AbilityEffectAnvilStrike : IAbilityEffect
    {
        private Scheduler _scheduler;
        private float _attackTime;
        private float _lastStartTime;
        private Action _cancelNext;

        private UnitResourceInterface _unitResourceInterface;
        private IEventBus _eventBus;

        private bool _disabled; // 👈 NEW FLAG

        public AbilityEffectAnvilStrike(Scheduler scheduler, UnitResourceInterface unitResourceInterface, IEventBus bus)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _unitResourceInterface = unitResourceInterface;
            _eventBus = bus;
        }

        public void Init(AbilityModifier abilityModifier)
        {
            _disabled = false; // reset if reused
            _attackTime = Mathf.Max(0.01f, abilityModifier.GetAttackTime());
            _unitResourceInterface.AddResource(ResourceId.AnvilStrike, new ResourceAnvilStrike(_eventBus));
            StartNewCycle(delay: _attackTime);
        }

        public void RefreshAbilityModifier(AbilityModifier abilityModifier)
        {
            //TODO fix
            return;
        }

        public void ManualOnDisable()
        {
            _disabled = true;
            _cancelNext?.Invoke();
            _cancelNext = null;
        }

        private void StartNewCycle(float delay)
        {
            if (_disabled) return; // 👈 don’t start new cycles if disabled
            _lastStartTime = Time.time;
            RescheduleOneShot(delay);
        }

        private void RescheduleOneShot(float delay)
        {
            if (_disabled) return; // 👈 safety check

            _cancelNext?.Invoke();

            float d = Mathf.Max(0.01f, delay);
            Action localCancel = null;

            localCancel = _scheduler.Every(d, () =>
            {
                // Make sure we weren’t disabled between scheduling and trigger
                if (_disabled)
                {
                    localCancel?.Invoke();
                    return;
                }

                localCancel?.Invoke();
                localCancel = null;

                OnStrike();

                // Restart cycle if still active
                if (!_disabled)
                    StartNewCycle(_attackTime);
            });

            _cancelNext = localCancel;
        }

        private void OnStrike()
        {
            if (_disabled) return;
            ((ResourceAnvilStrike)_unitResourceInterface
                    .GetUnitResource(ResourceId.AnvilStrike))
                .Increment();
        }
    }
}