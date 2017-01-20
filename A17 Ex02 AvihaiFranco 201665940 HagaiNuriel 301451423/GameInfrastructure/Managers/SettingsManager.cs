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
    public class SettingsManager : GameService, ISettingsManager
    {
        public event EventHandler MutedChange;

        public event EventHandler MouseVisibilityChange;

        public event EventHandler ResizeableChange;

        public event EventHandler FullScreenChange;

        private GraphicsDeviceManager m_GraficsDeviceManager;

        public int Level { get; set; }

        public float BGMusicVolume { get; set; }

        public float SoundFXVolume { get; set; }

        public int NumOfPlayers { get; set; }

        public bool SoundsMuted
        {
            get
            {
                return MediaPlayer.IsMuted;
            }

            set
            {
                MediaPlayer.IsMuted = value;
                muted_OnChange();
            }
        }

        public bool IsMouseVisible
        {
            get
            {
                return Game.IsMouseVisible;
            }
 
            set
            {
                Game.IsMouseVisible = value;
                mouseVisibility_OnChange();
            }
        }

        public bool IsResizeable
        {
            get
            {
                return Game.Window.AllowUserResizing;
            }

            set
            {
                Game.Window.AllowUserResizing = value;
                resizeable_OnChange();
            }
        }

        public bool IsFullScreen
        {
            get
            {
                return m_GraficsDeviceManager.IsFullScreen;
            }

            set
            {
                m_GraficsDeviceManager.IsFullScreen = value;
                m_GraficsDeviceManager.ApplyChanges();
                fullScreen_OnChange();
            }
        }

        public SettingsManager(Game i_Game)
            : base(i_Game)
        {
            m_GraficsDeviceManager =
                    Game.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
            SoundsMuted = IsFullScreen = false;
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

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ISettingsManager), this);
        }

        public void ToggleSounds()
        {
            SoundsMuted = !SoundsMuted;
        }

        private void muted_OnChange()
        {
            if (MutedChange != null)
            {
                MutedChange(null, EventArgs.Empty);
            }
        }

        public void ToggleMouseVisibility()
        {
            IsMouseVisible = !IsMouseVisible;
        }

        private void mouseVisibility_OnChange()
        {
            if (MouseVisibilityChange != null)
            {
                MouseVisibilityChange(null, EventArgs.Empty);
            }
        }

        public void ToggleWindowResizeable()
        {
            Game.Window.AllowUserResizing = !Game.Window.AllowUserResizing;
        }

        private void resizeable_OnChange()
        {
            if (ResizeableChange != null)
            {
                ResizeableChange(null, EventArgs.Empty);
            }
        }

        public void ToggleFullScreen()
        {
            if (m_GraficsDeviceManager != null)
            {
                IsFullScreen = !IsFullScreen;
            }
        }

        private void fullScreen_OnChange()
        {
            if (FullScreenChange != null)
            {
                FullScreenChange(null, EventArgs.Empty);
            }
        }
    }
}
