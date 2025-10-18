using System;
using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.HealthDisplay;
using Units.Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battlefield
{
    public class EnemyUnitSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;

        private static int _enemiesSpawned = 0;
        private Scheduler _scheduler;
        private BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;

        private int _spawnIntensity;

        private EnemySpawnLayerManager _layerManager;

        public void Init(Scheduler scheduler, BattlefieldInterfaceForUnit battlefieldInterfaceForUnit)
        {
            _scheduler = scheduler;
            _battlefieldInterfaceForUnit = battlefieldInterfaceForUnit;

            var trickleLayer = new SimpleTrickleEnemySpawnLayer();
            trickleLayer.Configure(
                startDelaySeconds: 2f,
                baseIntervalSeconds: 5f,
                minIntervalSeconds: 2f,
                intensityForMaxInterval: 0,
                intensityForMinInterval: 100,
                Spawn);

            var burstLayer = new BurstCircleEnemySpawnLayer()
                .Configure(
                    startDelaySeconds: 5f,
                    baseIntervalSeconds: 30f, // fixed 30s cadence
                    minIntervalSeconds: 30f,
                    intensityForMaxInterval: 0,
                    intensityForMinInterval: 100,
                    spawnCallback: null);
                ((BurstCircleEnemySpawnLayer) burstLayer).WithBurst(count: 5, radius: 1.0f)
                .WithCenter(new Vector3(-5, -5, 0)) // e.g., 6m ahead
                .WithSpawnAt(SpawnAt); // <-- this is the key line

            _layerManager = new EnemySpawnLayerManager();
            _layerManager.AddLayer(trickleLayer);
            _layerManager.AddLayer(burstLayer);
            _layerManager.StartAll(scheduler, new IntensityProvider(this));
        }

        void OnDisable()
        {
            _layerManager.StopAll();
        }


        public void Spawn() // legacy/default: still works
        {
            // pick your default placement (kept for trickle etc.)
            var pos = RandomPointOnCircleXZ(Vector3.zero, 6f);
            SpawnAt(pos);
        }

        public void SpawnAt(Vector3 position) // NEW: position-aware entry point
        {
            SpawnEnemy(position);
            _enemiesSpawned++;
            _spawnIntensity++; // if you want to count intensity per unit
        }

        private void SpawnEnemy(Vector3 position) // changed to take a position
        {
            GameObject enemyObject = Instantiate(enemyPrefab, position, Quaternion.identity);
            Unit enemyUnit = enemyObject.AddComponent<Unit>();
            InitEnemy(enemyUnit);
        }

        private void InitEnemy(Unit enemyUnit)
        {
            var singleTargetClosestAbility = new SingleTargetClosestAbility(
                transform,
                Faction.Player,
                2,
                _scheduler,
                10,
                LayerMask.GetMask("Ally"),
                _battlefieldInterfaceForUnit);


            var singleTargetAbility = new Ability(singleTargetClosestAbility);
            List<Ability> abilites = new List<Ability>();
            //abilites.Add(singleTargetAbility);

            var _abilityManager = new AbilityManager(abilites);

            UnitResourceManager unitResourceManager = new UnitResourceManager();
   
            enemyUnit.Init(
                10,
                new DefaultAbilityModifierSetProducer(),
                Faction.Enemy,
                _battlefieldInterfaceForUnit,
                _abilityManager,
                enemyUnit.transform,
                new DummyHealthDIsplayer(),
                unitResourceManager,
                new TriggerManager()
            );
        }

        public static Vector3 RandomPointOnCircleXZ(Vector3 center, float radius)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float x = center.x + Mathf.Cos(angle) * radius;
            float y = center.y + Mathf.Sin(angle) * radius;
            return new Vector3(x, y, center.z);
        }

        public int GetIntensity()
        {
            return _spawnIntensity;
        }
    }
}