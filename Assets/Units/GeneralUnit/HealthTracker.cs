using System;
using UnityEngine;

namespace Units
{
    public class HealthTracker
    {
        public int MaxHp { get; private set; }
        public int CurrentHp { get; private set; }

        public event Action OnDied;

        public HealthTracker(int initMaxHp)
        {
            MaxHp = initMaxHp;
            CurrentHp = initMaxHp;
        }


        public void TakeDamage(int amount)
        {
            CurrentHp -= amount;
            
            float percentageHealth = (float)CurrentHp / MaxHp;
            if (percentageHealth < 0)
            {
                percentageHealth = 0;
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

        public float GetPercentageHealth()
        {
            return CurrentHp / (float)MaxHp;
        }
    }
}