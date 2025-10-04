using Battlefield;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Battlefield.GameMechanics.Combat.BattlefieldController;
using UnityEngine;

namespace Units.GeneralUnit.Minion
{
    public class MinionSpawner : MonoBehaviour
    {
        [Header("Assign in Inspector")] [SerializeField] private GameObject _minionPrefab;

        [SerializeField] private BattlefieldController _battlefieldController;
        [SerializeField] private Scheduler _scheduler;
        
        public void SpawnMinion()
        {
            GameObject instance = Instantiate(_minionPrefab, new Vector2(3, 3),
                Quaternion.identity);
            
            
            instance.GetComponent<MinionUnit>().ApplySpawnInfo(
                _battlefieldController.GetBattlefieldKnowledge(),
                Faction.Player,
                _scheduler
                );
            
        }

        void Awake()
        {
            Debug.Log("Awake");
            SpawnMinion();
        }
    }
}