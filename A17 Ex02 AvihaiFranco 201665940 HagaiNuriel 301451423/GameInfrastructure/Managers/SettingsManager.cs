using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace GameInfrastructure.Managers
{
    public class SettingsManager: GameService, ISettingsManager
    {
        public event EventHandler MutedChange;
        public bool SoundsMuted { get; set; }
        public float BGMusicVolume { get; set; }
        public float SoundFXVolume { get; set; }
        public int NumOfPlayers { get; set; }

        public SettingsManager(Game i_Game)
            : base(i_Game)
        {
            SoundsMuted = false;
            BGMusicVolume = SoundFXVolume = 1f;
            NumOfPlayers = 1;
        }

        public void BGMusicVolumeUp(float i_VolumeToAdd)
        {
            MediaPlayer.Volume = BGMusicVolume = MathHelper.Clamp(BGMusicVolume + i_VolumeToAdd, 0f, 1f);
        }

        public void BGMusicVolumeDown(float i_VolumeToDecrease)
        {
            MediaPlayer.Volume = BGMusicVolume = MathHelper.Clamp(BGMusicVolume - i_VolumeToDecrease, 0f, 1f);
        }

        public void SoundFXVolumeUp(float i_VolumeToAdd)
        {
            SoundFXVolume = MathHelper.Clamp(SoundFXVolume + i_VolumeToAdd, 0f, 1f);
        }
        public void SoundFXVolumeDown(float i_VolumeToDecrease)
        {
            SoundFXVolume = MathHelper.Clamp(SoundFXVolume - i_VolumeToDecrease, 0f, 1f);
        }

        public void ToggleSound()
        {
            MediaPlayer.IsMuted = SoundsMuted = !SoundsMuted;
            if (MutedChange != null)
            {
                MutedChange(null, EventArgs.Empty);
            }

        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ISettingsManager), this);
        }
    }
}
