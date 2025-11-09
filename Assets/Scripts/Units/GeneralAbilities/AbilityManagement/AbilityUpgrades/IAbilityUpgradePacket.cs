using System.Collections.Generic;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;

namespace Units.GeneralAbilities.AbilityManagement.AbilityUpgrades
{
    public interface IAbilityUpgradePacket
    {
        void ApplyUpgrade(IAbilityUpgrade upgrade);
    }
}