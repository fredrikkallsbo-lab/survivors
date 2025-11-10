using System;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;

namespace Units.Abilities.AbilityManagement.AbilityUpgrades
{
    public interface IAbilityUpgrade
    {
        public String GetUpgradeString();
        
        public AbilityId GetAbilityId();
    }
}