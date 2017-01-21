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
    public class MainMenuScreen : MenuScreen
    {
        private Background m_Background;
        private ISettingsManager m_SettingsManager;

        public MainMenuScreen(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            TextComponent MainMenuTextComponent = new TextComponent(Game, "Main Menu", @"Fonts/Consolas");
            ChooseableMenuItem SoundOptions = new ChooseableMenuItem(Game, "Sound Options", @"Fonts/Consolas", Color.Blue, Color.Red);
            ChooseableMenuItem ScreenOptions = new ChooseableMenuItem(Game, "Screen Options", @"Fonts/Consolas", Color.Blue, Color.Red);
            SettingMenuItem PlayersOption = new SettingMenuItem(Game, "Players", @"Fonts/Consolas", Color.Blue, Color.Red);
            ChooseableMenuItem PlayOption = new ChooseableMenuItem(Game, "Play", @"Fonts/Consolas", Color.Blue, Color.Red);
            ChooseableMenuItem QuitOption = new ChooseableMenuItem(Game, "Quit", @"Fonts/Consolas", Color.Blue, Color.Red);
            MainMenuTextComponent.Scales = new Vector2(4, 5);
            MainMenuTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 150);
            MainMenuTextComponent.AlignToCenter();
            m_Background = new Background(this.Game, ObjectValues.BackgroundTextureString);
            m_SettingsManager = Game.Services.GetService(typeof(ISettingsManager)) as ISettingsManager;
            SelectionChangeSoundEffect = Game.Content.Load<SoundEffect>(@"C:/Temp/XNA_Assets/Ex03/Sounds/MenuMove");
            this.Add(m_Background);
            SoundOptions.Choose += onSoundOptionsScreen;
            SoundOptions.Scales = Vector2.One * 2f;
            ScreenOptions.Scales = Vector2.One * 2f;
            PlayersOption.Scales = Vector2.One * 2f; 
            PlayOption.Scales = Vector2.One * 2f; 
            QuitOption.Scales = Vector2.One * 2f;
            ScreenOptions.Choose += onScreensOptions;
            PlayersOption.ExtraText = m_SettingsManager.NumOfPlayers == 1 ? "One" : "Two";
            PlayersOption.ToggleDown += onChangePlayerCount;
            PlayersOption.ToggleUp += onChangePlayerCount;
            PlayOption.Choose += onPlay;
            QuitOption.Choose += onQuit;
            Add(MainMenuTextComponent);
            Add(SoundOptions);
            Add(ScreenOptions);
            Add(PlayersOption);
            Add(PlayOption);
            Add(QuitOption);
            base.Initialize();
        }

        private void onScreensOptions(object i_Sender, EventArgs i_EventArgs)
        {
            m_ScreensManager.SetCurrentScreen(new ScreenOptionsScreen(Game));
        }

        private void onPlay(object i_Sender, EventArgs i_EventArgs)
        {
            this.ExitScreen();
            this.ScreensManager = Game.Services.GetService(typeof(IScreensMananger)) as IScreensMananger;
            (ScreensManager as ScreensMananger).Push(new PlayScreen(Game));
            ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
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

        private void onSoundOptionsScreen(object i_Sender, EventArgs i_EventArgs)
        {
            ScreensManager.SetCurrentScreen(new SoundOptionsScreen(Game));
        }
    }
}
