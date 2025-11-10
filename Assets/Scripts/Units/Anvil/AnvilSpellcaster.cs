using System.Collections.Generic;
using Battlefield.Combat.BattlefieldController;
using Battlefield.Combat.StatusEffectManagement;
using Units.Abilities;
using Units.Abilities.AbilityManagement;
using Units.Abilities.AbilityManagement.AbilityGeneral;
using Units.Anvil.AnvilAbilities;
using Units.Anvil.AnvilAbilities.Chains;
using Units.GeneralAbilities;
using Units.GeneralUnit.Minion;
using Units.GeneralUnit.Spawning.EnemySpawning;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units.Anvil
{
    public class AnvilSpellcaster : MonoBehaviour
    {
        private ExpandingCircle _expandingCirclePrefab;
        private BattlefieldController  _battlefieldController;
        private GeneralSpawner generalSpawner;

        public void Init(ExpandingCircle prefab, BattlefieldController battlefieldController)
        {
            _expandingCirclePrefab = prefab;
            _battlefieldController = battlefieldController;
            generalSpawner = _battlefieldController.GetGeneralSpawner();
        }

        
        /*
         * Uppgraderingar:
         *      +radius
         *      +/- speed
         *      +damage
         *      frequency
         *      Unika förmågor? chans att applicera fiery rune?
         *      en svagare version som åker tillbaka
         */
        public void CastMagmaWave(
            float maxRadius, 
            float expansionSpeed, 
            int damage
            )
        {
    
            Debug.Log("Casting magma Wave, maxRadius: " + maxRadius + ", expansionSpeed: " +  expansionSpeed + ", damage: " + damage);
            var p = new ExpandingCircle.Params
            {
                position       = transform.position,
                parent         = null,               // or a transform to parent under
                startRadius    = 0f,
                maxRadius      = maxRadius,
                expansionSpeed = expansionSpeed,               // units/sec
                initialDelay   = 0f,               // wait before expanding
                damagePerHit   = damage,
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
        }

        /*
         * Uppgraderingar:
         *      duration?
         *      damage
         *      explosions area
         *      + %chans att påverka extra target
         *      
         */
        
        public void CastFieryRune()
        {
            Unit targetUnit = _battlefieldController.GetRandomUnit(Faction.Enemy, StatusEffectId.FieryRune);
            if (targetUnit == null) return;
            targetUnit.ReceiveStatusEffect(new StatusEffectFieryRune(targetUnit, _battlefieldController.GetEventBus()));
        }

        /*
         * Uppgraderingar:
         *      + max units
         *      + duration?
         *      + damage
         *      + hp
         *      + movement speed
         *      + unika buffs på elemental? RF? damage conversion? splash attack?
         *      + frequency
         */
        
        public void CastMagmaElemental()
        {
            Vector2 pos = RandomPointOnCircleXZ(transform.position, 2);
            List<IAbility> abilities = new List<IAbility>();
            
            generalSpawner.SpawnUnit(
                pos,
                "Sprites/Random/MagmaElemental",
                "Player",
                "MagmaElemental",
                new AbilityManager(abilities),
                Faction.Player
            );
        }
        
        public static Vector3 RandomPointOnCircleXZ(Vector3 center, float radius)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float x = center.x + Mathf.Cos(angle) * radius;
            float y = center.y + Mathf.Sin(angle) * radius;
            return new Vector3(x, y, center.z);
        }

        
        
        
        /*
         * Uppgraderingar:
         *      +targets
         *      range
         *      pull strength
         *      frequency of cast
         *      reverse pull? - borde vara på gear
         */
        public void CastChains()
        {
            
            Debug.Log("Casting chains");
            List<Unit> chainOfUnits = _battlefieldController.GetChainOfUnitsStartingFromRandomUnitInRange(
                Faction.Enemy,
                25f,
                25f,
                4,
                StatusEffectId.Chain);

            if (chainOfUnits.Count < 2)
            {
                return;
            }
            
            List<StatusEffectChain> chains = new List<StatusEffectChain>();
            
            foreach (Unit unit in chainOfUnits)
            {
                chains.Add(new StatusEffectChain());
            }
            
            for (int i = 0; i < chains.Count; i++)
            {
                if (i == 0)
                {
                    chains[0].ConstructChainAnchors(chainOfUnits[i], null, chains[1]);
                } 
                else if (i == chains.Count - 1)
                {
                    chains[^1].ConstructChainAnchors(chainOfUnits[i], chains[^2], null);
                }
                else
                {
                    chains[i].ConstructChainAnchors(chainOfUnits[i], chains[i-1], chains[i+1]);
                } 
            }
            
            for(int i = 0; i < chains.Count; i++)
            {
               chainOfUnits[i].ReceiveStatusEffect(chains[i]);
            }
            
        }
    }
}