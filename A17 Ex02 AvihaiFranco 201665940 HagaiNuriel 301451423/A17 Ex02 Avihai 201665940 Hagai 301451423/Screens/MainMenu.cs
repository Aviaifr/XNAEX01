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
    public class MainMenu : MenuScreen
    {
        private Background m_Background;
        public MainMenu(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            m_Background = new Background(this.Game, ObjectValues.BackgroundTextureString);
            this.Add(m_Background);
            ChooseableMenuItem SoundOptions = new ChooseableMenuItem(Game, "Sound Options", @"Fonts/Consolas", Color.Blue, Color.Red);
            SoundOptions.Choose += onSoundOptionsScreen;
            ChooseableMenuItem ScreenOptions = new ChooseableMenuItem(Game, "Screen Options", @"Fonts/Consolas", Color.Blue, Color.Red);
            SettingMenuItem PlayersOption = new SettingMenuItem(Game, "Players", @"Fonts/Consolas", Color.Blue, Color.Red);
            PlayersOption.ExtraText = "One";
            PlayersOption.ToggleDown += onChangePlayerCount;
            PlayersOption.ToggleUp += onChangePlayerCount;
            ChooseableMenuItem PlayOption = new ChooseableMenuItem(Game, "Play", @"Fonts/Consolas", Color.Blue, Color.Red);
            PlayOption.Choose += onPlay;
            ChooseableMenuItem QuitOption = new ChooseableMenuItem(Game, "Quit", @"Fonts/Consolas", Color.Blue, Color.Red);
            QuitOption.Choose += onQuit;

            this.AddOption(SoundOptions);
            this.AddOption(ScreenOptions);
            this.AddOption(PlayersOption);
            this.AddOption(PlayOption);
            this.AddOption(QuitOption);
            base.Initialize();
        }

        private void onPlay(object i_Sender, EventArgs i_EventArgs)
        {
            this.OnClosed();
        }

        private void onQuit(object i_Sender, EventArgs i_EventArgs)
        {
            Game.Exit();
        }

        private void onChangePlayerCount(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem togglePlayer = i_Sender as SettingMenuItem;
            if (togglePlayer != null)
            {
                if (togglePlayer.ExtraText.Contains("One"))
                {
                    togglePlayer.ExtraText = "Two";
                }
                else
                {
                    togglePlayer.ExtraText = "One";
                }
            }
        }

        private void onSoundOptionsScreen(object i_Sender, EventArgs i_EventArgs)
        {
            m_ScreensManager.SetCurrentScreen(new SoundOptionsScreen(Game));
        }

    }
}
