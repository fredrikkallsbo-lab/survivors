using Battlefield.GameMechanics.Combat.AbilityModifying;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units;
using Units.Enemy;
using UnityEngine;

namespace Battlefield
{
    public class EnemyUnitSpawner : MonoBehaviour
    {
        [Header("Assign in Inspector")] [SerializeField]
        private GameObject enemyPrefab;

        private static int _enemiesSpawned = 0;
        
        Scheduler scheduler;
        System.Action cancel;

        [SerializeField] BattlefieldController battlefieldController;
        
        void OnEnable()
        {
            scheduler = FindObjectOfType<Scheduler>();
            cancel = scheduler.Every(5.0, Spawn);
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
            if (enemyPrefab == null)
            {
                Debug.LogError("EnemySpawner: enemyPrefab is not assigned.", this);
                return;
            }

            GameObject instance = Instantiate(enemyPrefab, RandomPointOnCircleXZ(new Vector3(0, 0, 0), 6),
                Quaternion.identity);
            
            instance.GetComponent<EnemyUnit>().ApplySpawnInfo(10 + _enemiesSpawned,
                new DefaultAbilityModifierSetProducer(), 
                battlefieldController.GetBattlefieldKnowledge(),
                scheduler);
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