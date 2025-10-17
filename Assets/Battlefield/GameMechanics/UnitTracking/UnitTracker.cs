using System.Collections.Generic;
using Battlefield.GameMechanics;
using Battlefield.GameMechanics.Combat.loot;
using Units;
using Units.Player;
using UnityEngine;

namespace Battlefield
{
    public class UnitTracker : MonoBehaviour
    {
        private static List<Unit> _units = new();
      
        
        public void Register(Unit unit)
        {
            _units.Add(unit);
        }

        public void Unregister(Unit unit)
        {
            _units.Remove(unit);
            CheckWinOrLose();
        }

        public void CheckWinOrLose()
        {
            bool playerIsAlive = false;
            bool enemyIsAlive = false;

            foreach (var unit in _units)
            {
                if (unit.Faction == Faction.Player)
                {
                    playerIsAlive = true;
                }
                else if(unit.Faction == Faction.Enemy)
                {
                    enemyIsAlive = true;
                }
            }

            if (!playerIsAlive)
            {
                Debug.Log("DEFEAT");
            }else if (!enemyIsAlive)
            {
                Debug.Log("VICTORY");
            }
        }
        
    }
}