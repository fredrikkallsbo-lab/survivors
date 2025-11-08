using System;
using System.Collections.Generic;
using Battlefield;
using Battlefield.Combat.BattlefieldController;
using Battlefield.GameMechanics;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Anvil;
using Units.Anvil.AnvilAbilities;
using Units.Anvil.AnvilAbilities.MagmaElemental;
using Units.Anvil.AnvilAbilities.MagmaWave;
using Units.Death;
using Units.GeneralAbilities;
using Units.GeneralUnit.Minion;
using Units.HealthDisplay;
using Units.Resources;
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

        private PlayerScreenHealthText _playerScreenHealthText;
        private void Awake()
        {
            enemyUnitSpawner.Init(scheduler, battlefieldController.GetBattlefieldUnitInterface(), battlefieldController.GetEventBus());
            
            _playerScreenHealthText = FindObjectOfType<PlayerScreenHealthText>();
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
            List<Ability> abilities = new List<Ability>();


            GameObject go = new GameObject("AnvilSpellcaster", typeof(AnvilSpellcaster));
            go.transform.position = playerUnit.transform.position;
            
            var spellcaster = go.GetComponent<AnvilSpellcaster>();
            Debug.Assert(expandingCirclePrefab != null,
                "UnitSpawner: expandingCirclePrefab is not assigned in Inspector.");
            spellcaster.Init(expandingCirclePrefab, battlefieldController);

            
            
            
            AbilityEffectMagmaWave effectMagmaWave = new AbilityEffectMagmaWave(
                triggerManager,
                battlefieldController.GetEventBus(),
                spellcaster
            );
            Ability magmaWave = new Ability(effectMagmaWave);

            
            
            AbilityEffectFieryRune effectFieryRune = new AbilityEffectFieryRune(
                spellcaster,
                triggerManager,
                battlefieldController.GetEventBus());
            Ability fieryRune = new Ability(effectFieryRune);
            
            
            AbilityEffectMagmaElemental effectmagmaElemental = new AbilityEffectMagmaElemental(
                spellcaster,
                triggerManager,
                battlefieldController.GetEventBus());
            Ability magmaElemental = new Ability(effectmagmaElemental);
            
            abilities.Add(anvilStrikeAbility);
            abilities.Add(magmaWave);
            abilities.Add(fieryRune);
            abilities.Add(magmaElemental);

            var _abilityManager = new AbilityManager(abilities);
            var _wanderer = new Wanderer(playerUnit, battlefieldController.GetEventBus());

            playerUnit.Init(
                1000,
                _wanderer.CreateAbilityModifier(),
                Faction.Player,
                battlefieldController.GetBattlefieldUnitInterface(),
                _abilityManager,
                _playerScreenHealthText,
                unitResourceManager,
                triggerManager,
                battlefieldController.GetEventBus(),
                new PlayerDeathEventCreator()
                );
            battlefieldController.GetBattlefieldUnitInterface().RegisterWanderer(_wanderer);
        }
    }
}