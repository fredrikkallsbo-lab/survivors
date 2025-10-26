using Battlefield.Combat.DamageCalculation.DamagePackets;

namespace Battlefield.Combat.DamageCalculation.DamagePipeline
{
    public class DamagePipeline
    {
        public PreDefensePipelineDamagePacket ApplyPipeline(PreDamagePipelineDamagePacket preDamagePipelineDamagePacket)
        {
            return new PreDefensePipelineDamagePacket(preDamagePipelineDamagePacket.GetPacket().CreateCopy());
        }
    }
}