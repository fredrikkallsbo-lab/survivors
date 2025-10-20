using System.Collections.Generic;
using Battlefield.GameMechanics;
using Battlefield.GameMechanics.Combat.loot;
using Units;
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
        }
        
    }
}