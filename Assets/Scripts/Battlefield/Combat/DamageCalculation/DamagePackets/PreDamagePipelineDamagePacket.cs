namespace Battlefield.Combat.DamageCalculation.DamagePackets
{
    public class PreDamagePipelineDamagePacket
    {
        private DamagePacket _damagePacket;

        public DamagePacket GetPacket()
        {
            return _damagePacket;
        }
    }
}