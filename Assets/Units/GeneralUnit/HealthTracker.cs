using System;
using UnityEngine;

namespace Units
{
    public class HealthTracker
    {
        public int MaxHp { get; private set; }
        public int CurrentHp { get; private set; }

        private CircleFillOverlay _fillOverlay;

        public event Action OnDied;

        public HealthTracker(int initMaxHp, CircleFillOverlay circleFillOverlay)
        {
            MaxHp = initMaxHp;
            CurrentHp = initMaxHp;
            _fillOverlay = circleFillOverlay;
        }


        public void TakeDamage(int amount)
        {
            CurrentHp -= amount;
            
            float percentageHealth = (float)CurrentHp / MaxHp;
            if (percentageHealth < 0)
            {
                percentageHealth = 0;
            }

            if (_fillOverlay != null)
            {
                _fillOverlay.SetFill(percentageHealth);
            }
        }

        public bool IsDead()
        {
            if (CurrentHp <= 0)
            {
                OnDied?.Invoke();
                return true;
            }

            return false;
        }
    }
}