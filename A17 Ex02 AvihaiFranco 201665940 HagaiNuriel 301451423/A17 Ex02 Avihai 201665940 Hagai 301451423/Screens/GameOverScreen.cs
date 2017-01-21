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
    public class GameOverScreen : GameScreen
    {
        private TextComponent m_endGameText;

        public GameOverScreen(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            Background background = new Background(Game, ObjectValues.BackgroundTextureString);
            background.Tint = Color.Red;
            Add(background);
            TextComponent gameOverTextComponent = new TextComponent(Game, "Game Over", @"Fonts/Consolas");
            gameOverTextComponent.Tint = Color.DarkRed;
            gameOverTextComponent.Scale = new Vector2(5, 6);
            gameOverTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 150);
            gameOverTextComponent.AlignToCenter();
            Add(gameOverTextComponent);
            m_endGameText = new TextComponent(Game, string.Empty, @"Fonts/Consolas");
            m_endGameText.Position = new Vector2(100, 250);
            m_endGameText.Scale = Vector2.One * 2.5f;
            m_endGameText .Tint = Color.Gold;
            Add(m_endGameText);
            TextComponent startGameTextComponent = new TextComponent(Game,
@"Press 'Home' To Start A New Game
Press 'M' For Main Menu
Press 'Esc' To Quit",
                    @"Fonts/Consolas");
            startGameTextComponent.Tint = Color.GhostWhite;
            startGameTextComponent.Scale = Vector2.One * 1.7f;
            startGameTextComponent.AlignToCenter();
            startGameTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 500); ;
            Add(new MothershipEnemy(Game, ObjectValues.MothershipTextureString, 0));
            Add(startGameTextComponent);
            base.Initialize();
        }

        protected override void OnActivated()
        {
            if ((Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager).GetPlayerByIndex(0) != null)
            {
                updateScoreText();
            }

            base.OnActivated();
        }

        private void updateScoreText()
        {
            List<string> winnerList = new List<string>();
            IPlayersManager playersManager = Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
            int numOfPlayers = (Game.Services.GetService(typeof(ISettingsManager)) as ISettingsManager).NumOfPlayers;
            int maxScore = 0;
            string msg = string.Format("Scores:{0}", Environment.NewLine);
            for (int i = 0; i < numOfPlayers; i++)
            {
                Player player = playersManager.GetPlayerByIndex(i) as Player;
                msg = string.Format("{0}{1} : {2}{3}", msg, player.PlayerId, player.Score.ToString(), Environment.NewLine);
                if (player.Score > maxScore)
                {
                    maxScore = player.Score;
                    winnerList.Clear();
                    winnerList.Add(player.PlayerId);
                }
                else if (player.Score == maxScore)
                {
                    winnerList.Add(player.PlayerId);
                }
            }

            if (winnerList.Count >= 2)
            {
                msg = string.Format("{0}Tie!", msg);
            }
            else
            {
                msg = string.Format("{0}{1} Wins!", msg, winnerList[winnerList.Count - 1]);
            }
            m_endGameText.Text = msg;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }
            else if (InputManager.KeyPressed(Keys.M))
            {
                this.ScreensManager.SetCurrentScreen(new MainMenuScreen(Game));
            }
            else if (InputManager.KeyPressed(Keys.Home))
            {
                (ScreensManager as ScreensMananger).Push(new PlayScreen(Game));
                this.ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
            }
            base.Update(gameTime);
        }
    }
}
