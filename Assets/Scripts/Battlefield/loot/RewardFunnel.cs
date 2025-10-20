
using UnityEngine;

namespace Battlefield.GameMechanics.Combat.loot
{
    public class RewardFunnel
    {
        private Wanderer _wanderer;

        public RewardFunnel(Wanderer wanderer)
        {
            _wanderer = wanderer;
        }

        public void AddExperience(int i)
        {
            _wanderer.AddExperience(i);
        }
    }
}