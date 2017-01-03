using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class SpaceInvaderGame : Game
    {
        public static int EnemyRows = 5;
        public static int EnemyCols = 9;
        private GraphicsDeviceManager m_Graphics;
        private List<SpaceShipPlayer> m_Players = new List<SpaceShipPlayer>();
        private SpriteBatch m_SpriteBatch;
        private Background m_Background;
        private EnemyBatch m_EnemyBatch;
        private PlayersManager m_PlayersManager;

        private int PointsCollected
        {
            get;
            set; 
        }

        public void Enemy_OnKill(object i_EnemyKilled, EventArgs i_eventArgs)
        {
            if (this.Components != null)
            {
                SpaceShipPlayer player = m_Players.Find(spaceShipPlayer =>
                (spaceShipPlayer.GameComponent as UserSpaceship) == (i_EnemyKilled as Enemy).KilledBy);
                player.Score += (i_EnemyKilled as Enemy).Value;
                if(i_EnemyKilled is MothershipEnemy)
                {
                    MothershipEnemy motherShip = i_EnemyKilled as MothershipEnemy;
                    motherShip.Velocity = Vector2.Zero;
                    motherShip.ActivateAnimation(ObjectValues.DeathAnimation);
                }
            }
        }

        public void enemy_OnDisposed(object i_Disposed, EventArgs i_EventArgs)
        {
            if (m_EnemyBatch.EnemyCount == 0)
            {
                GameOver();
            }
        }

        public void Player_OnHit(object i_HitPlayer, EventArgs i_EventArgs)
        {
            enablePlayerSpaceshipAnimation(i_HitPlayer as SpaceShipPlayer);
        }

        private void enablePlayerSpaceshipAnimation(SpaceShipPlayer i_Player)
        {
            Sprite playerComponent = i_Player.GameComponent as Sprite;
            playerComponent.ActivateAnimation(ObjectValues.HitAnimation);
            playerComponent.Position = (playerComponent as UserSpaceship).BeginningPosition;
        }

        public void Player_OnKilled(object i_HitPlayer, EventArgs i_EventArgs)
        {
            SpaceShipPlayer player = i_HitPlayer as SpaceShipPlayer;
            Sprite playerSprite = player.GameComponent as Sprite;

            if (playerSprite != null && playerSprite.isCollidable)
            {
                playerSprite.isCollidable = false;
                playerSprite.Animations.Enable(ObjectValues.DeathAnimation);
            }

            checkGameOver();
        }

        private void checkGameOver()
        {
            bool isGameOver = true;
            foreach (Player player in m_Players)
            {
                if (player.Lives > 0)
                {
                    isGameOver = false;
                }
            }

            if (isGameOver == true)
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            string message;
            List<string> winnerList = new List<string>();
            int maxScore = 0;
            string msg = string.Format("Scores:{0}", Environment.NewLine);
            foreach (Player player in m_Players)
            {
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

            System.Windows.Forms.MessageBox.Show(msg, "Game Over");
            this.Exit();
        }

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
            m_Background = new Background(this, ObjectValues.BackgroundTextureString);
            Components.Add(m_Background);
            initPlayers();

            m_EnemyBatch = new EnemyBatch(this);
            m_EnemyBatch.EnemyKilled += Enemy_OnKill;
            m_EnemyBatch.EnemyReachedBottom += Enemy_OnReachBottom;
            m_EnemyBatch.NoMoreEnemies += enemy_OnDisposed;
            Components.Add(m_EnemyBatch);

            WallBatch wallBatch = new WallBatch(this);
            Components.Add(wallBatch);

            MothershipEnemy mothershipEnemy = new MothershipEnemy(this, ObjectValues.MothershipTextureString, ObjectValues.MothershipValue);
            mothershipEnemy.Position = new Vector2(0, ObjectValues.EnemyWidth);
            mothershipEnemy.MothershipKilled += Enemy_OnKill;
            Components.Add(mothershipEnemy);

            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);
            initManagers();
            base.Initialize();
        }

        private void initPlayers()
        {
            int textureIndex = 0;
            Vector2 startingPosition = 
                new Vector2(0, this.GraphicsDevice.Viewport.Height - ObjectValues.SpaceshipSize);
            Vector2 scorePosition = new Vector2(5, 20);
            SpaceShipPlayer player;
            UserSpaceship spaceShip;
            ScoreBoard scoreBoard;
            foreach(string playerId in ObjectValues.PlayerIds)
            {
                spaceShip = 
                    new UserSpaceship(this, ObjectValues.SpaceShipTextures[textureIndex], playerId, startingPosition);
                spaceShip.Position = startingPosition;
                spaceShip.Shoot += spaceship_Shot;
                this.Components.Add(spaceShip);

                player = new SpaceShipPlayer(spaceShip, playerId);
                player.PlayerHit += Player_OnHit;
                player.PlayerDead += Player_OnKilled;

                string scoreBoardText = "P" + (textureIndex + 1) + " Score: ";
                scoreBoard = new ScoreBoard(this, scoreBoardText, ObjectValues.ConsolasFont);
                scoreBoard.Position = scorePosition;
                scoreBoard.Tint = ObjectValues.ScoreBoardsColors[textureIndex];
                player.ScoreBoard = scoreBoard;
                this.Components.Add(scoreBoard);

                m_Players.Add(player); 
                textureIndex++;
                startingPosition.X += ObjectValues.SpaceshipSize;
                scorePosition.Y += 20;
            }

            SoulsBatch player1Souls = new SoulsBatch(this, Color.White, new Vector2(GraphicsDevice.Viewport.Width - 80, 20));
            Components.Add(player1Souls);
            SoulsBatch player2Souls = new SoulsBatch(this, Color.ForestGreen, new Vector2(GraphicsDevice.Viewport.Width - 80, 45));
            Components.Add(player2Souls);
            m_Players[0].SoulBatch = player1Souls;
            m_Players[1].SoulBatch = player2Souls;
        }

        private void initManagers()
        {
            new InputManager(this);
            new CollisionsManager(this);
            m_PlayersManager = initPlayersManager();
        }

        private PlayersManager initPlayersManager()
        {
            PlayersManager playersManager = new PlayersManager(this);
            PlayerInfo player = new PlayerInfo();
            foreach(string playerId in ObjectValues.PlayerIds)
            { 
              player = mapPlayer(playerId);
              playersManager.PlyersInfo.Add(playerId, player);
            }

            return playersManager;
        }

        private PlayerInfo mapPlayer(string i_PlayerId)
        {
            PlayerInfo playerInfo = new PlayerInfo();

            Dictionary<eActions, eInputButtons> mouseGamePadDic =
                playerInfo.MouseGamepadDictionary;
            Dictionary<eActions, Keys> KeyboardDic =
                playerInfo.KeyBoardDictionary;

            switch(i_PlayerId)
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

        private Vector2 getEnemyPosition(int i_row, int i_col)
        {
            float y = (3 * ObjectValues.EnemyWidth) + ((1.6f * ObjectValues.EnemyWidth) * i_row);
            float x = (1.6f * ObjectValues.EnemyWidth) * i_col;
            return new Vector2(x, y);
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        private void spaceship_Shot(object i_Sender, EventArgs i_EventArgs)
        {
            SpaceBullet newBullet = new SpaceBullet(this, ObjectValues.BulletTextureString, ObjectValues.UserShipBulletTint, -1);
            newBullet.Initialize();
            newBullet.Owner = i_Sender as UserSpaceship;
            setNewSpaceshipBulletPosition(i_Sender as UserSpaceship, newBullet);
            newBullet.Disposed += (i_Sender as UserSpaceship).OnMyBulletDisappear;
            newBullet.Disposed += onComponentDisposed;
            this.Components.Add(newBullet);
        }

        private void setNewSpaceshipBulletPosition(UserSpaceship i_Spaceship, SpaceBullet i_newBullet)
        {
            Vector2 newBulletPos = i_Spaceship.Position;
            newBulletPos.X += i_Spaceship.Width / 2;
            newBulletPos.X -= i_newBullet.Width / 2;
            newBulletPos.Y -= i_newBullet.Height;
            i_newBullet.Position = newBulletPos;
        }

        public void onComponentDisposed(object i_Disposed, EventArgs i_EventArgs)
        {
            if (Components != null)
            {
                Components.Remove(i_Disposed as IGameComponent);
            }
        }

        public void Enemy_OnReachBottom(object i_ReacedBottomObj, EventArgs i_EventArgs)
        {
            GameOver();
        }
    }
}
