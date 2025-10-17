using System.Collections.Generic;
using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.HealthDisplay;
using UnityEngine;

namespace Units.GeneralUnit.Minion
{
    public class MinionSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _minionPrefab;

        [SerializeField] private BattlefieldController _battlefieldController;
        [SerializeField] private Scheduler _scheduler;
        
        public void SpawnMinion()
        {
            GameObject minion = Instantiate(_minionPrefab, new Vector2(3, 3), Quaternion.identity);
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
            
            var singleTargetAbility = new Ability(singleTargetProjectileAbility);
            List<Ability> abilites = new List<Ability>();
            abilites.Add(singleTargetAbility);
            
            var _abilityManager = new AbilityManager(abilites);
            
            minionUnit.Init(
                10,
                new DefaultAbilityModifierSetProducer(),
                Faction.Enemy,
                _battlefieldController.GetBattlefieldUnitInterface(),
                _abilityManager,
                minionUnit.transform,
                new DummyHealthDIsplayer());
        }
        


        public void Init()
        {
            SpawnMinion();
        }
    }
}