using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Screens;
using GameInfrastructure.Menu;

namespace Space_Invaders.Screens
{
    public class SoundOptionsScreen : MenuScreen
    {
        private Background m_Background;
        public SoundOptionsScreen(Game i_Game):base(i_Game)
        {
        }

        public override void Initialize()
        {
            SettingMenuItem ToggleSound;
            SettingMenuItem SoundFXOption;
            SettingMenuItem MusicOption;
            m_Background = new Background(this.Game, ObjectValues.BackgroundTextureString);
            this.Add(m_Background);
            SoundFXOption = new SettingMenuItem(Game, "Sounds Effects Volume", @"Fonts/Consolas", Color.Blue, Color.Red);
            SoundFXOption.ExtraText = "100";
            SoundFXOption.ToggleUp += onVolumeUp;
            SoundFXOption.ToggleDown += onVolumeDown;
            MusicOption = new SettingMenuItem(Game, "Background Music Volume", @"Fonts/Consolas", Color.Blue, Color.Red);
            MusicOption.ExtraText = "100";
            MusicOption.ToggleUp += onVolumeUp;
            MusicOption.ToggleDown += onVolumeDown;
            ToggleSound = new SettingMenuItem(Game, "Toggle Sound", @"Fonts/Consolas", Color.Blue, Color.Red);
            ToggleSound.ExtraText = "On";
            ToggleSound.ToggleUp += onToggleSounds;
            ToggleSound.ToggleDown += onToggleSounds;
            ChooseableMenuItem Done = new ChooseableMenuItem(Game, "Done", @"Fonts/Consolas", Color.Blue, Color.Red);
            Done.Choose += onChooseDone; 
            this.AddOption(MusicOption);
            this.AddOption(SoundFXOption);
            this.AddOption(ToggleSound);
            this.AddOption(Done);
            base.Initialize();
        }

        private void onChooseDone(object i_Sender, EventArgs i_EventArgs)
        {
            this.OnClosed();
        }

        private void onToggleSounds(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem toggledOption = i_Sender as SettingMenuItem;
            if (toggledOption != null)
            {
                if (toggledOption.ExtraText.Contains("On"))
                {
                    toggledOption.ExtraText = "Off";
                }
                else
                {
                    toggledOption.ExtraText = "On";
                }
            }
        }

        private void onVolumeUp(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem volumeOption = i_Sender as SettingMenuItem;
            int currVolume;
            if (volumeOption != null)
            {
                currVolume = int.Parse(volumeOption.ExtraText.Remove(0, 3));
                currVolume = MathHelper.Clamp(currVolume + 10, 0, 100);
                volumeOption.ExtraText = currVolume.ToString();
            }
        }

        private void onVolumeDown(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem volumeOption = i_Sender as SettingMenuItem;
            int currVolume;
            if (volumeOption != null)
            {
                currVolume = int.Parse(volumeOption.ExtraText.Remove(0, 3));
                currVolume = MathHelper.Clamp(currVolume - 10, 0, 100);
                volumeOption.ExtraText = currVolume.ToString();
            }
        }
    }
}
