using Units;

namespace Battlefield.Combat.StatusEffectManagement
{
    public interface IStatusEffect
    {
        public void ApplyStatusEffect(Unit targetUnit);

        public StatusEffectId GetStatusEffectId();
        public void RemoveStatusEffect();
    }
}