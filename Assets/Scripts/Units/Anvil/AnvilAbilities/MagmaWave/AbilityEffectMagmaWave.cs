using System.Collections.Generic;
using Battlefield.GameMechanics.Combat.AbilityModifying;
using Units.Anvil.AnvilAbilities.MagmaWave.Upgrade;
using Units.GeneralAbilities.AbilityManagement.AbilityGeneral;
using UnityEngine;

namespace Units.Anvil.AnvilAbilities.MagmaWave
{
    public class AbilityEffectMagmaWave : IAbilityEffect
    {
        private TriggerManager _triggerManager;
        private IEventBus _eventBus;
        private TriggerMagmaWaveOnStrike _triggerMagmaWaveOnStrike;
        private AnvilSpellcaster _anvilSpellcaster;
        

        //Base values
        private float _baseRadius = 3;
        private float _baseSpeed = 2;
        private int _baseDamage = 25;
        private int _baseFrequency = 2;
        
        public AbilityEffectMagmaWave(TriggerManager triggerManager,
            IEventBus eventBus,
            AnvilSpellcaster spellcaster
            )
        {
            _triggerManager = triggerManager;
            _eventBus = eventBus;
            _anvilSpellcaster = spellcaster;   
        }

        public void Init(AbilityModifier abilityModifier)
        {

            AbilityUpgradePacketMagmaWave packet = 
                (AbilityUpgradePacketMagmaWave) abilityModifier.GetAbilityUpgradePacket(AbilityId.MagmaWave);
            _triggerMagmaWaveOnStrike = new TriggerMagmaWaveOnStrike(_eventBus,  _anvilSpellcaster, 
                _baseFrequency + packet.Frequency);
            RefreshAbilityModifier(abilityModifier);
            _triggerManager.Add(_triggerMagmaWaveOnStrike);
        }

        public void RefreshAbilityModifier(AbilityModifier abilityModifier)
        {
            AbilityUpgradePacketMagmaWave packet = 
                (AbilityUpgradePacketMagmaWave) abilityModifier.GetAbilityUpgradePacket(AbilityId.MagmaWave);

            float maxRadius = (_baseRadius + packet.FlatBonusRadius) * packet.IncreaseRadius;
            float speed = _baseSpeed * packet.SpeedModifier;
            int damage = (int)((_baseDamage + packet.FlatDamage) * packet.IncreaseDamage);
            
            Debug.Log("Updating values : maxRadius: " + maxRadius + ", speed: " + speed + ", damage: " + damage);
            _triggerMagmaWaveOnStrike.UpdateValues(
                maxRadius, 
                speed, 
                damage
                );
        }

        public void ManualOnDisable()
        {
            _triggerManager.Remove(_triggerMagmaWaveOnStrike);
        }

        
    }
}