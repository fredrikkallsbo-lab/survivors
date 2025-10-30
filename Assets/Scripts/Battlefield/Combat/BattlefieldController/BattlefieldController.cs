using System.Collections.Generic;
using Battlefield.Combat.StatusEffectManagement;
using Battlefield.GameMechanics;
using Battlefield.GameMechanics.Combat.loot;
using Units;
using UnityEngine;
using Random = System.Random;

namespace Battlefield.Combat.BattlefieldController
{
    public class BattlefieldController: MonoBehaviour
    {
        [SerializeField] private UnitTracker unitTracker;
        
        private BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;
        private readonly IEventBus  _eventBus = new EventBus();
        private RewardFunnel _rewardFunnel;


        public void RegisterWanderer(Wanderer wanderer)
        {
            _rewardFunnel = new RewardFunnel(wanderer);
        }

        public void RegisterUnit(Unit unit)
        {
            unitTracker.Register(unit);
        }

        public void UnregisterUnit(Unit unit)
        {
            unitTracker.Unregister(unit);
//            _rewardFunnel.AddExperience(1);
            Destroy(unit.gameObject);
        }


        public BattlefieldInterfaceForUnit GetBattlefieldUnitInterface()
        {
           return new BattlefieldInterfaceForUnit(this);
        }

        public IEventBus GetEventBus()
        {
            return _eventBus;
        }

        public Unit GetRandomUnit(Faction targetFaction, StatusEffectId statusEffectId)
        {
            List<Unit> units = unitTracker.GetUnits();
            
            List<Unit> factionUnits = new List<Unit>();

            
            foreach (var unit in units)
            {
                if (unit.faction == targetFaction && !unit.HasStatusEffect(statusEffectId))
                {
                    factionUnits.Add(unit);
                }
            }
            if (factionUnits.Count == 0)
            {
                return null;
            }
            int index = new Random().Next(0, factionUnits.Count);
            return factionUnits[index];
        }
    }
}