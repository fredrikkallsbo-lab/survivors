using System.Collections.Generic;
using UnityEngine;

namespace Battlefield.GameMechanics.Combat.CombatScripting
{
    [CreateAssetMenu(menuName = "Battle/Script")]
    public class BattleScript : ScriptableObject
    {
        public int seed = 12345;
        private List<BattlePhase> _phases;
        
        
    }

}