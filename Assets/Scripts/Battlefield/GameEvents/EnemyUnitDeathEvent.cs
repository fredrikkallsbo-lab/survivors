using System.Collections.Generic;
using Units;

namespace Battlefield.GameEvents
{
    public class EnemyUnitDeathEvent : IGameEvent
    {
        private Unit _unit;

        public EnemyUnitDeathEvent(Unit unit)
        {
            _unit = unit;
        }


        public Unit GetUnit()
        {
            return _unit;
        }
    }
}