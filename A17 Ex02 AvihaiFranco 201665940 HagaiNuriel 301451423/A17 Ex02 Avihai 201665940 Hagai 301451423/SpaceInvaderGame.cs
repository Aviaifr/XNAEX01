using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using Space_Invaders.Screens;

namespace Space_Invaders
{
    public class SpaceInvaderGame : Game
    {
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private PlayersManager m_PlayersManager;
        private ScreensMananger m_ScreenManager;
        private SettingsManager m_settingsManager;
        private InputManager m_inputManager;
        private Song m_BGMusicSong;
        
        public SpaceInvaderGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            m_Graphics.PreferredBackBufferHeight = 600;
            m_Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.Window.Title = "Space Invaders";
            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);
            initManagers();
            new SoundEffectsPlayer(this, m_settingsManager);
            initScreens();
            base.Initialize();
        }
        
        private void initScreens()
        {
            m_ScreenManager.SetCurrentScreen(new WellcomeScreen(this));
        }

        private void initManagers()
        {
            m_inputManager = new InputManager(this);
            new CollisionsManager(this);
            m_settingsManager = new SettingsManager(this);
            m_ScreenManager = new ScreensMananger(this);
            m_PlayersManager = initPlayersManager();
        }

        private PlayersManager initPlayersManager()
        {
            PlayersManager playersManager = new PlayersManager(this);
            PlayerInfo player = new PlayerInfo();
            foreach (string playerId in ObjectValues.PlayerIds)
            {
                player = mapPlayer(playerId);
                playersManager.PlyersInfo.Add(playerId, player);
            }

            return playersManager;
        }

        protected override void LoadContent()
        {
            m_BGMusicSong = Content.Load<Song>(System.IO.Path.GetFullPath(@"C:/Temp/XNA_Assets/Ex03/Sounds/BGMusic"));
            MediaPlayer.Play(m_BGMusicSong);
            MediaPlayer.IsRepeating = true;
            base.LoadContent();
        }

        private PlayerInfo mapPlayer(string i_PlayerId)
        {
            PlayerInfo playerInfo = new PlayerInfo();

            Dictionary<eActions, eInputButtons> mouseGamePadDic =
                playerInfo.MouseGamepadDictionary;
            Dictionary<eActions, Keys> KeyboardDic =
                playerInfo.KeyBoardDictionary;

            switch (i_PlayerId)
            {
                case ObjectValues.k_PlayerOneId:
                    playerInfo.IsMouseControlled = true;
                    KeyboardDic.Add(eActions.Left, Keys.Left);
                    KeyboardDic.Add(eActions.Right, Keys.Right);
                    KeyboardDic.Add(eActions.Shoot, Keys.Up);
                    mouseGamePadDic.Add(eActions.Shoot, eInputButtons.Left);
                    break;
                case ObjectValues.k_PlayerTwoId:
                    playerInfo.IsMouseControlled = false;
                    KeyboardDic.Add(eActions.Left, Keys.D);
                    KeyboardDic.Add(eActions.Right, Keys.G);
                    KeyboardDic.Add(eActions.Shoot, Keys.R);
                    break;
            }

            return playerInfo;
        }

        protected override void Update(GameTime gameTime)
        {
            if(m_inputManager.KeyPressed(Keys.OemMinus))
            {
                m_settingsManager.ToggleSounds();
            }

            base.Update(gameTime);
        }
    }
}
