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
        public MainMenu(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            ChooseableMenuItem SoundOptions = new ChooseableMenuItem(Game, "Sound Options", @"Fonts/Consolas", Color.Blue, Color.Red);
            ChooseableMenuItem ScreenOptions = 
                new ChooseableMenuItem(Game, "Screen Options", @"Fonts/Consolas", Color.Blue, Color.Red);
            ScreenOptions.Choose += onScreensOptions;
            SettingMenuItem PlayersToggle = new SettingMenuItem(Game, "Players", @"Fonts/Consolas", Color.Blue, Color.Red);
            PlayersToggle.ExtraText = "One";
            ChooseableMenuItem PlayOption = new ChooseableMenuItem(Game, "Play", @"Fonts/Consolas", Color.Blue, Color.Red);
            PlayOption.Choose += onPlay;
            ChooseableMenuItem QuitOption = new ChooseableMenuItem(Game, "Quit", @"Fonts/Consolas", Color.Blue, Color.Red);
            
            this.AddOption(SoundOptions);
            this.AddOption(ScreenOptions);
            this.AddOption(PlayersToggle);
            this.AddOption(PlayOption);
            this.AddOption(QuitOption);
            base.Initialize();
        }

        private void onPlay(object i_Sender, EventArgs i_EventArgs)
        {
            this.OnClosed();
        }

        private void onScreensOptions(object i_Sender, EventArgs i_EventArgs)
        {
            m_ScreensManager.SetCurrentScreen(new ScreenOptionsScreen(Game));
        }
    }
}
