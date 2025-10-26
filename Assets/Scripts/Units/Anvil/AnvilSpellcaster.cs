using Battlefield.Combat.BattlefieldController;
using Battlefield.Combat.StatusEffectManagement;
using Units.Abilities;
using Units.Anvil.AnvilAbilities;
using Units.GeneralAbilities;
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
    
            var p = new ExpandingCircle.Params
            {
                position       = transform.position,
                parent         = null,               // or a transform to parent under
                startRadius    = 0f,
                maxRadius      = 6f,
                expansionSpeed = 0.7f,               // units/sec
                initialDelay   = 0f,               // wait before expanding
                damagePerHit   = 40f,
                destroyOnMax   = true,
                stopAtMax      = true,

                // Optional overrides
                drawRing       = true,
                ringSegments   = 64,
                ringWidth      = 0.06f,
                ringColor      = new Color(1f, 0.4f, 0.1f, 1f),
                ringSortingLayer = "Effects",
                ringSortingOrder = 10,
                effectMode     = ExpandingCircle.EffectMode.Push,
                effectStrength = 5f,
                forceMode      = ForceMode2D.Impulse,
                targetLayers   = LayerMask.GetMask("Enemy"),
                reactToTriggers = true
            };
            var circle = ExpandingCircle.Spawn(p);

           // Vector3 pos = transform.position;   // << from caster/player, not mouse
            //Instantiate(_expandingCirclePrefab, pos, Quaternion.identity);
            
        }

        public void CastFieryRune()
        {
            Debug.Log("Casting Fiery rune");
            Unit targetUnit = _battlefieldController.GetRandomUnit(Faction.Enemy, StatusEffectId.FieryRune);
            if (targetUnit == null) return;
            targetUnit.ReceiveStatusEffect(new StatusEffectFieryRune(targetUnit, _battlefieldController.GetEventBus()));
        }
    }
}