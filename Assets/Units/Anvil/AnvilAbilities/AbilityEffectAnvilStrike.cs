using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Anvil.AnvilResources;
using Units.Resources;
using UnityEngine;

namespace System.Collections.Generic
{
    public class AbilityEffectAnvilStrike : IAbilityEffect
    {
        private Scheduler _scheduler;

        // Current timing
        private float _attackTime; // seconds (from AbilityModifierSet)
        private float _lastStartTime; // Time.time when current swing cycle started
        private Action _cancelNext; // cancels the pending one-shot

        private UnitResourceInterface _unitResourceInterface;
        private IEventBus _eventBus;
        // Hook: what to do when the strike triggers
       // private readonly Action _onStrike;

        public AbilityEffectAnvilStrike(Scheduler scheduler, UnitResourceInterface unitResourceInterface, IEventBus bus)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _unitResourceInterface = unitResourceInterface;
            _eventBus = bus;

            //  _onStrike = onStrike ?? throw new ArgumentNullException(nameof(onStrike));
        }

        public void Init(AbilityModifierSet abilityModifierSet)
        {
            // Start a fresh cycle based on current AttackTime
            _attackTime = Mathf.Max(0.01f, abilityModifierSet.GetAttackTime());
            _unitResourceInterface.AddResource(ResourceId.AnvilStrike, new ResourceAnvilStrike(_eventBus));
            StartNewCycle(delay: _attackTime);
        }

        public void RefreshAbilityModifierSet(AbilityModifierSet abilityModifierSet)
        {
            // Preserve elapsed fraction; scale remaining by NEW attack time
            float newAttack = Mathf.Max(0.01f, abilityModifierSet.GetAttackTime());
            float now = Time.time;

            // How far into the current cycle are we (0..1)?
            float elapsed = Mathf.Max(0f, now - _lastStartTime);
            float oldLen = Mathf.Max(0.01f, _attackTime);
            float frac = Mathf.Clamp01(elapsed / oldLen);

            // Remaining portion under new timing
            float remainingFrac = 1f - frac;
            float newRemaining = remainingFrac * newAttack;

            // Update and reschedule from NOW
            _attackTime = newAttack;

            // Keep the cycle’s conceptual start aligned with the preserved fraction:
            // lastStartTime = now - (elapsedFrac * newAttack)
            _lastStartTime = now - (frac * newAttack);

            RescheduleOneShot(newRemaining);
        }

        public void ManualOnDisable()
        {
            _cancelNext?.Invoke();
            _cancelNext = null;
        }

        // -------- internals --------

        private void StartNewCycle(float delay)
        {
            _lastStartTime = Time.time;
            RescheduleOneShot(delay);
        }

        private void RescheduleOneShot(float delay)
        {
            // Cancel any pending tick, then schedule a one-shot via Every(...)
            _cancelNext?.Invoke();

            float d = Mathf.Max(0.01f, delay);
            Action localCancel = null;
            localCancel = _scheduler.Every(d, () =>
            {
                // turn Every into one-shot
                localCancel?.Invoke();
                localCancel = null;

                // Fire the strike and start the next cycle with current _attackTime
                //_onStrike?.Invoke();
                OnStrike();
                StartNewCycle(_attackTime);
            });

            _cancelNext = localCancel;
        }

        private void OnStrike()
        {
            Debug.Log("OnStrike");
            ((ResourceAnvilStrike)_unitResourceInterface.GetUnitResource(ResourceId.AnvilStrike)).Increment();
        }
        
    }
}