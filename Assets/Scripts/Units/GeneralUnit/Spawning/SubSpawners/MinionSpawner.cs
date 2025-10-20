using System.Collections.Generic;
using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Death;
using Units.HealthDisplay;
using Units.Resources;
using UnityEngine;

namespace Units.GeneralUnit.Minion
{
    public class MinionSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _minionPrefab;

        [SerializeField] private BattlefieldController _battlefieldController;
        [SerializeField] private Scheduler _scheduler;
        
        public void SpawnMinion(Vector3 spawnPosition)
        {
            GameObject minion = Instantiate(_minionPrefab, spawnPosition, Quaternion.identity);
            Unit minionUnit = minion.AddComponent<Unit>();
            InitMinion(minionUnit);

        }

        private void InitMinion(Unit minionUnit)
        {
            var singleTargetProjectileAbility = new SingleTargetProjectileAbility(
                minionUnit.transform,
                Faction.Player, 
                2, 
                _scheduler,
                100,
                LayerMask.GetMask("Enemy"),
                _battlefieldController.GetBattlefieldUnitInterface());

            UnitResourceManager unitResourceManager = new UnitResourceManager();
            var singleTargetAbility = new Ability(singleTargetProjectileAbility);
            List<Ability> abilites = new List<Ability>();
            abilites.Add(singleTargetAbility);
            
            var _abilityManager = new AbilityManager(abilites);
            
            minionUnit.Init(
                10,
                new AbilityModifierSet(0),
                Faction.Player,
                _battlefieldController.GetBattlefieldUnitInterface(),
                _abilityManager,
                minionUnit.transform,
                new DummyHealthDIsplayer(),
                unitResourceManager,
                new TriggerManager(),
                _battlefieldController.GetEventBus(),
                new EnemyUnitDeathEventCreator());
        }
    }
}