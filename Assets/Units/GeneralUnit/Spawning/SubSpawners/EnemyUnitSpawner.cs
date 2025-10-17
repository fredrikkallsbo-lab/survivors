using System;
using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.HealthDisplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battlefield
{
    public class EnemyUnitSpawner: MonoBehaviour
    {
        
        [SerializeField] private GameObject enemyPrefab;
        
        private static int _enemiesSpawned = 0;
        private Scheduler _scheduler;
        private BattlefieldInterfaceForUnit _battlefieldInterfaceForUnit;
        
        Action cancel;

        
        public void Init(Scheduler scheduler, BattlefieldInterfaceForUnit battlefieldInterfaceForUnit)
        {
            _scheduler = scheduler;
            _battlefieldInterfaceForUnit = battlefieldInterfaceForUnit;
            cancel = _scheduler.Every(5.0, Spawn);
        }

        void OnDisable()
        {
            // Cancel when this object goes inactive or destroyed
            cancel?.Invoke();
            cancel = null;
        }

        void Spawn()
        {
            SpawnEnemy();
            _enemiesSpawned++;
        }

        public void SpawnEnemy()
        {
            GameObject enemyObject = Instantiate(enemyPrefab, RandomPointOnCircleXZ(new Vector3(0, 0, 0), 6),
                Quaternion.identity);
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

            
            //CircleFillOverlay circle = enemyUnit.gameObject.AddComponent<CircleFillOverlay>();
           // circle.SetSprite(circleSprite);
            enemyUnit.Init(
                10,
                new DefaultAbilityModifierSetProducer(),
                Faction.Enemy,
                _battlefieldInterfaceForUnit,
                _abilityManager,
                enemyUnit.transform,
                new DummyHealthDIsplayer()
                );
        }
        
        public static Vector3 RandomPointOnCircleXZ(Vector3 center, float radius)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float x = center.x + Mathf.Cos(angle) * radius;
            float y = center.y + Mathf.Sin(angle) * radius;
            return new Vector3(x, y, center.z);
        }
    }
}