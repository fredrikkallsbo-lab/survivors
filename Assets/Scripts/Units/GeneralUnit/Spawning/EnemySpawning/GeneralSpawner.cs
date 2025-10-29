using System;
using System.Collections.Generic;
using Battlefield.Combat.BattlefieldController;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Death;
using Units.GeneralUnit.Movement;
using Units.HealthDisplay;
using Units.Resources;
using UnityEngine;

namespace Units.GeneralUnit.Spawning.EnemySpawning
{
    public class GeneralSpawner : MonoBehaviour
    {

        [SerializeField] private BattlefieldController  _battlefieldController;


        public void Awake()
        {
            Vector2 pos = new Vector2(2f, 2f);
            List<Ability> abilities = new List<Ability>();
            
            SpawnUnit(
                pos,
                "Sprites/Random/MagmaElemental",
                "Player",
                "MagmaElemental",
                new AbilityManager(abilities),
                Faction.Player
                );
        }

        private void SpawnUnit(
            Vector2 position, 
            string spritePath, 
            string unityTag,
            string unitName,
            AbilityManager abilityManager,
            Faction faction
            )
        {
            Debug.Log("Spawning Unit");
            GameObject unitObject = new GameObject(unitName);
            unitObject.transform.position = position;

            Unit unit = unitObject.AddComponent<Unit>();
            unit.Init(
                5,
                new AbilityModifier(0),
                faction,
                _battlefieldController.GetBattlefieldUnitInterface(),
                abilityManager,
                new DummyHealthDIsplayer(),
                new UnitResourceManager(),
                new TriggerManager(),
                _battlefieldController.GetEventBus(),
                new EnemyUnitDeathEventCreator()
            );

            Rigidbody2D rigidbody2D = unitObject.AddComponent<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D.gravityScale = 0f;
            
            CircleCollider2D collider = unitObject.AddComponent<CircleCollider2D>();
            collider.radius = 0.5f;
            
            SpriteRenderer sr = unitObject.AddComponent<SpriteRenderer>();
            Sprite sprite = UnityEngine.Resources.Load<Sprite>(spritePath);
            sr.sprite = sprite;
            
            unitObject.layer = LayerMask.NameToLayer("Ally");
            unitObject.tag = unityTag;
            
            MeleeChase meleeChase = unitObject.gameObject.AddComponent<MeleeChase>();
            meleeChase.Init(
                _battlefieldController.GetBattlefieldUnitInterface(),
                Faction.Enemy,
                1,
                LayerMask.GetMask("Enemy"),
                1f);   
            _battlefieldController.GetBattlefieldUnitInterface().RegisterSpawn(unit);
        }
        
    }
}