using Units.Abilities;
using Unity.VisualScripting;
using UnityEngine;

namespace Units.Anvil
{
    public class AnvilSpellcaster : MonoBehaviour
    {
        private ExpandingCircle _expandingCirclePrefab;

        public void Init(ExpandingCircle prefab)
        {
            _expandingCirclePrefab = prefab;
        }

        public void CastMagmaWave()
        {
            if (_expandingCirclePrefab == null) { Debug.LogError("No prefab"); return; }

            Vector3 pos = transform.position;   // << from caster/player, not mouse
            var circle = Instantiate(_expandingCirclePrefab, pos, Quaternion.identity);
            
        }
    }
}