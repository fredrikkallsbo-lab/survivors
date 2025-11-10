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
using Units.Anvil.AnvilAbilities.AnvilStrike;
using Units.Anvil.AnvilAbilities.Chains;
using Units.Anvil.AnvilAbilities.MagmaElemental;
using Units.Anvil.AnvilAbilities.MagmaWave;
using Units.Death;
using Units.GeneralAbilities;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;
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
            enemyUnitSpawner.Init(scheduler, battlefieldController.GetBattlefieldUnitInterface(),
                battlefieldController.GetEventBus());

            _playerScreenHealthText = FindObjectOfType<PlayerScreenHealthText>();
            GameObject player = Instantiate(playerUnitPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Unit playerUnit = player.AddComponent<Unit>();
            InitPlayer(playerUnit);
        }

        private void InitPlayer(Unit playerUnit)
        {
            UnitResourceManager unitResourceManager = new UnitResourceManager();
            TriggerManager triggerManager = new TriggerManager();

            AbilityAnvilStrike abilityAnvilStrike = new AbilityAnvilStrike(scheduler,
                unitResourceManager.GetUnitResourceInterface(),
                battlefieldController.GetEventBus());

            List<IAbility> abilities = new List<IAbility>();


            GameObject go = new GameObject("AnvilSpellcaster", typeof(AnvilSpellcaster));
            go.transform.position = playerUnit.transform.position;

            var spellcaster = go.GetComponent<AnvilSpellcaster>();
            Debug.Assert(expandingCirclePrefab != null,
                "UnitSpawner: expandingCirclePrefab is not assigned in Inspector.");
            spellcaster.Init(expandingCirclePrefab, battlefieldController);


            IAbility magmaWave = new AbilityMagmaWave(triggerManager,
                battlefieldController.GetEventBus(),
                spellcaster);


            AbilityEffectFieryRune effectFieryRune = new AbilityEffectFieryRune(
                spellcaster,
                triggerManager,
                battlefieldController.GetEventBus());
            //IAbility fieryRune = new IAbility(effectFieryRune, AbilityId.FieryRune);


            AbilityEffectMagmaElemental effectmagmaElemental = new AbilityEffectMagmaElemental(
                spellcaster,
                triggerManager,
                battlefieldController.GetEventBus());
            //IAbility magmaElemental = new IAbility(effectmagmaElemental,  AbilityId.MagmaElemental);


            AbilityEffectChains effectChains = new AbilityEffectChains(
                spellcaster,
                triggerManager,
                battlefieldController.GetEventBus());
            //IAbility chains = new IAbility(effectChains, AbilityId.MoltenChains);

            abilities.Add(abilityAnvilStrike);
            abilities.Add(magmaWave);
            // abilities.Add(fieryRune);
            // abilities.Add(magmaElemental);
            //abilities.Add(chains);

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