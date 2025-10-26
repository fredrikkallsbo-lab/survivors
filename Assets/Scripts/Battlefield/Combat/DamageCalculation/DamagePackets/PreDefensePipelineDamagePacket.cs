namespace Battlefield.Combat.DamageCalculation.DamagePackets
{
    public class PreDefensePipelineDamagePacket
    {
        private DamagePacket _damagePacket;

        public PreDefensePipelineDamagePacket(DamagePacket damagePacket)
        {
            _damagePacket = damagePacket;
        }
        
        public DamagePacket GetPacket()
        {
            return _damagePacket;
        }
        
    }
}