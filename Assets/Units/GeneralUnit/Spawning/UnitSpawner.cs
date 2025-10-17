using System;
using System.Collections.Generic;
using Battlefield;
using Battlefield.GameMechanics;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.GeneralUnit.Minion;
using Units.HealthDisplay;
using Unity.VisualScripting;
using UnityEngine;

namespace Units
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyUnitSpawner enemyUnitSpawner;
        [SerializeField] private MinionSpawner minionSpawner;
        [SerializeField] private GameObject playerUnitPrefab;
        
        [SerializeField] private BattlefieldController battlefieldController;
        [SerializeField] private Scheduler scheduler;
        
        private void Awake()
        {
            enemyUnitSpawner.Init(scheduler, battlefieldController.GetBattlefieldUnitInterface());
            minionSpawner.Init();
            
            GameObject player = Instantiate(playerUnitPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Unit playerUnit = player.AddComponent<Unit>();
            InitPlayer(playerUnit);

        }

        private void InitPlayer(Unit playerUnit)
        {
            var singleTargetClosestAbility = new SingleTargetClosestAbility(
                transform, 
                Faction.Player, 
                2, 
                scheduler,
                10,
                LayerMask.GetMask("Enemy"),
                battlefieldController.GetBattlefieldUnitInterface());
            
            
            
            var singleTargetAbility = new Ability(singleTargetClosestAbility);
            List<Ability> abilites = new List<Ability>();
            //abilites.Add(singleTargetAbility);
            
            var _abilityManager = new AbilityManager(abilites);
            
            var _character = new Character();
            
            
            playerUnit.Init(
                100,
                _character,
                Faction.Player,
                battlefieldController.GetBattlefieldUnitInterface(),
                _abilityManager,
                playerUnit.transform,
                new DummyHealthDIsplayer()
                );
        }
    }
}