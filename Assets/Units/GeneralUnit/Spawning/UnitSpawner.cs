using System;
using System.Collections.Generic;
using Battlefield;
using Battlefield.GameMechanics;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Anvil;
using Units.Anvil.AnvilAbilities;
using Units.GeneralUnit.Minion;
using Units.HealthDisplay;
using Units.Resources;
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

        [SerializeField] private ExpandingCircle expandingCirclePrefab;

        private PlayerScreenHealthBar _playerScreenHealthBar;
        private void Awake()
        {
            enemyUnitSpawner.Init(scheduler, battlefieldController.GetBattlefieldUnitInterface());
            
            _playerScreenHealthBar = FindObjectOfType<PlayerScreenHealthBar>();
            GameObject player = Instantiate(playerUnitPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Unit playerUnit = player.AddComponent<Unit>();
            InitPlayer(playerUnit);
        }

        private void InitPlayer(Unit playerUnit)
        {
            UnitResourceManager unitResourceManager = new UnitResourceManager();
            TriggerManager triggerManager = new TriggerManager();
            AbilityEffectAnvilStrike effectAnvilStrike = new AbilityEffectAnvilStrike(scheduler,
                unitResourceManager.GetUnitResourceInterface(),
                battlefieldController.GetEventBus());
            Ability anvilStrikeAbility = new Ability(effectAnvilStrike);
            List<Ability> abilites = new List<Ability>();


            // DO NOT instantiate the circle here
            GameObject go = new GameObject("AnvilSpellcaster", typeof(AnvilSpellcaster));
            go.transform.position = playerUnit.transform.position;

            // pass the PREFAB to the spellcaster
            var spellcaster = go.GetComponent<AnvilSpellcaster>();
            Debug.Assert(expandingCirclePrefab != null,
                "UnitSpawner: expandingCirclePrefab is not assigned in Inspector.");
            spellcaster.Init(expandingCirclePrefab);

            // build your ability using this spellcaster    
            AbilityEffectMagmaWave effectMagmaWave = new AbilityEffectMagmaWave(
                triggerManager,
                battlefieldController.GetEventBus(),
                spellcaster
            );

            Ability magmaWave = new Ability(effectMagmaWave);

            abilites.Add(anvilStrikeAbility);
            //abilites.Add(magmaWave);

            var _abilityManager = new AbilityManager(abilites);
            var _wanderer = new Wanderer();

            playerUnit.Init(
                100,
                _wanderer,
                Faction.Player,
                battlefieldController.GetBattlefieldUnitInterface(),
                _abilityManager,
                playerUnit.transform,
                _playerScreenHealthBar,
                unitResourceManager,
                triggerManager
            );
        }
    }
}