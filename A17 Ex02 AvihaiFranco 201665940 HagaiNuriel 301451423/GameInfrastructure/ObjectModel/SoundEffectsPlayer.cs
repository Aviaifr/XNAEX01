using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using GameInfrastructure.ServiceInterfaces;

namespace GameInfrastructure.ObjectModel
{
    public class SoundEffectsPlayer : GameService, ISoundEffectsPlayer
    {
        ISettingsManager m_SettingsManager;
        public SoundEffectsPlayer(Game i_Game, ISettingsManager i_SettingsManager)
            : base(i_Game)
        {
            m_SettingsManager = i_SettingsManager;
        }
        
        public void PlaySoundEffect(SoundEffect i_EffectToPlay)
        {
            if (!m_SettingsManager.SoundsMuted)
            {
                SoundEffectInstance instance = i_EffectToPlay.CreateInstance();
                instance.Volume = m_SettingsManager.SoundFXVolume;
                instance.Play();
            }
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ISoundEffectsPlayer), this);
        }
    }
}
