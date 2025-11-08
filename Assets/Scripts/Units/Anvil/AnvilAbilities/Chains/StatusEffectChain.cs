using Battlefield.Combat.StatusEffectManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Units.Anvil.AnvilAbilities.Chains
{
    public class StatusEffectChain : IStatusEffect
    {
        private Unit _targetUnit;
        private StatusEffectChain _previousStatusEffect;
        private StatusEffectChain _nextStatusEffect;
        private ChainAnchor _chainAnchor;

     

        public void ConstructChainAnchors(Unit targetUnit, StatusEffectChain previousStatusEffect,  StatusEffectChain nextStatusEffect)
        {
            _targetUnit = targetUnit;
            _previousStatusEffect = previousStatusEffect;
            _nextStatusEffect = nextStatusEffect;
            
            _chainAnchor = _targetUnit.gameObject.AddComponent<ChainAnchor>();
        }
        
        public void ApplyStatusEffect(Unit targetUnit)
        {
            
            ChainAnchor previousChainAnchor = null;
            ChainAnchor nextChainAnchor = null;
            
            if (_previousStatusEffect != null)
            {
                previousChainAnchor = _previousStatusEffect.GetChainAnchor();
            }

            if (_nextStatusEffect != null)
            {
                nextChainAnchor = _nextStatusEffect.GetChainAnchor();
            }
            _chainAnchor.Init(previousChainAnchor, nextChainAnchor);
        }
        

        public StatusEffectId GetStatusEffectId()
        {
            return StatusEffectId.Chain;
        }

        public void RemoveStatusEffect()
        {
            // Case 1: middle of chain (has both)
            if (_previousStatusEffect != null && _nextStatusEffect != null)
            {
                _previousStatusEffect.RepairBrokenChainNext(_nextStatusEffect);
                _nextStatusEffect.RepairBrokenChainPrevious(_previousStatusEffect);
            }
            // Case 2: start of chain (no previous)
            else if (_previousStatusEffect == null && _nextStatusEffect != null)
            {
                _nextStatusEffect.BreakChainFromPrevious();
            }
            // Case 3: end of chain (no next)
            else if (_nextStatusEffect == null && _previousStatusEffect != null)
            {
                _previousStatusEffect.BreakChainFromNext();
            }

            Object.Destroy(_chainAnchor);
        }

        public ChainAnchor GetChainAnchor()
        {
            return _chainAnchor;
        }

        public void BreakChainFromPrevious()
        {
            _previousStatusEffect = null;

            if (_previousStatusEffect == null && _nextStatusEffect == null)
            {
                _targetUnit.RemoveStatusEffect(this);
                return;
            }

            _chainAnchor.Init(
                null,
                _nextStatusEffect.GetChainAnchor()
            );
        }
        public void BreakChainFromNext()
        {
            _nextStatusEffect = null;

            if (_previousStatusEffect == null && _nextStatusEffect == null)
            {
                _targetUnit.RemoveStatusEffect(this);
                return;
            }

            _chainAnchor.Init(
                _previousStatusEffect.GetChainAnchor(),
                null
            );
        }
        
        public void RepairBrokenChainPrevious(StatusEffectChain newPrevious)
        {
            _previousStatusEffect = newPrevious;
            _chainAnchor.Init(_previousStatusEffect?.GetChainAnchor(), _nextStatusEffect?.GetChainAnchor());
        }

        public void RepairBrokenChainNext(StatusEffectChain newNext)
        {
            _nextStatusEffect = newNext;
            _chainAnchor.Init(_previousStatusEffect?.GetChainAnchor(), _nextStatusEffect?.GetChainAnchor());
        }
    }
}