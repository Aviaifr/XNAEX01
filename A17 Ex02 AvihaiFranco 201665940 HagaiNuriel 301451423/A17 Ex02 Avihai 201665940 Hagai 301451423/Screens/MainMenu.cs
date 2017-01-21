using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
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
        private ISettingsManager m_SettingsManager;

        public MainMenu(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            m_Background = new Background(this.Game, ObjectValues.BackgroundTextureString);
            m_SettingsManager = Game.Services.GetService(typeof(ISettingsManager)) as ISettingsManager;
            //SelectionChangeSoundEffect = Game.Content.Load<SoundEffect>(System.IO.Path.GetFullPath(@"../../../../../../../../../Temp/XNA_Assets/Ex03/Sounds/MenuMove"));
            this.Add(m_Background);
            ChooseableMenuItem SoundOptions = new ChooseableMenuItem(Game, "Sound Options", @"Fonts/Consolas", Color.Blue, Color.Red);
            ChooseableMenuItem ScreenOptions = new ChooseableMenuItem(Game, "Screen Options", @"Fonts/Consolas", Color.Blue, Color.Red);
            SettingMenuItem PlayersOption = new SettingMenuItem(Game, "Players", @"Fonts/Consolas", Color.Blue, Color.Red);
            PlayersOption.ExtraText = m_SettingsManager.NumOfPlayers == 1 ? "One" : "Two";
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
            this.ScreensManager = Game.Services.GetService(typeof(IScreensMananger)) as IScreensMananger;
            ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game, 1));
            this.OnClosed();
        }

        private void onQuit(object i_Sender, EventArgs i_EventArgs)
        {
            Game.Exit();
        }

        private void onChangePlayerCount(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem togglePlayer = i_Sender as SettingMenuItem;
            if (togglePlayer.ExtraText.Contains("One"))
            {
                m_SettingsManager.NumOfPlayers = 2;
            }
            else
            {
                m_SettingsManager.NumOfPlayers = 1;
            }
            togglePlayer.ExtraText = m_SettingsManager.NumOfPlayers == 1 ? "One" : "Two";
        }
    }
}
