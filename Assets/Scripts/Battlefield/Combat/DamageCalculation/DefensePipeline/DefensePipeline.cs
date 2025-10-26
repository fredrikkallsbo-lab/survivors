using Battlefield.Combat.DamageCalculation.DamagePackets;

namespace Battlefield.Combat.DamageCalculation.DefensePipeline
{
    public class DefensePipeline
    {
        public int GetFinalDamage(PreDefensePipelineDamagePacket preDefensePipelineDamagePacket)
        {
            return preDefensePipelineDamagePacket.GetPacket().GetTotalDamage();
        }
    }
}