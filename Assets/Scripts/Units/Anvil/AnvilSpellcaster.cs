using Battlefield.Combat.BattlefieldController;
using Units.Abilities;
using Units.Anvil.AnvilAbilities;
using Unity.VisualScripting;
using UnityEngine;

namespace Units.Anvil
{
    public class AnvilSpellcaster : MonoBehaviour
    {
        private ExpandingCircle _expandingCirclePrefab;
        private BattlefieldController  _battlefieldController;

        public void Init(ExpandingCircle prefab, BattlefieldController battlefieldController)
        {
            _expandingCirclePrefab = prefab;
            _battlefieldController = battlefieldController;
        }

        public void CastMagmaWave()
        {
            if (_expandingCirclePrefab == null) { Debug.LogError("No prefab"); return; }

            Vector3 pos = transform.position;   // << from caster/player, not mouse
            Instantiate(_expandingCirclePrefab, pos, Quaternion.identity);
            
        }

        public void CastFieryRune()
        {
            Debug.Log("Casting Fiery rune");
            Unit targetUnit = _battlefieldController.GetRandomUnit(Faction.Enemy);
            targetUnit.ReceiveStatusEffect(new StatusEffectFieryRune());
        }
    }
}