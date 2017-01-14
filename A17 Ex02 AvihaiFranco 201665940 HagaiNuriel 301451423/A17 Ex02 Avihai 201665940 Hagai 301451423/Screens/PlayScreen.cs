using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Screens;

namespace Space_Invaders.Screens
{
    public class PlayScreen : GameScreen
    {
        public static int EnemyRows = 5;
        public static int EnemyCols = 9;
        private List<SpaceShipPlayer> m_Players = new List<SpaceShipPlayer>();
        private Background m_Background;
        private EnemyBatch m_EnemyBatch;

        public PlayScreen(Game i_Game):base(i_Game)
        {
        }

        private int PointsCollected
        {
            get;
            set; 
        }

        public void Enemy_OnKill(object i_EnemyKilled, EventArgs i_eventArgs)
        {
            SpaceShipPlayer player = m_Players.Find(spaceShipPlayer =>
                (spaceShipPlayer.GameComponent as UserSpaceship) == (i_EnemyKilled as Enemy).KilledBy);
            if (player != null)
            {
                
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
            //this.Exit(); TODO: when game is overS
        }

        public override void Initialize()
        {
            m_Background = new Background(this.Game, ObjectValues.BackgroundTextureString);
            this.Add(m_Background);
            initPlayers();

            m_EnemyBatch = new EnemyBatch(this.Game);
            m_EnemyBatch.EnemyKilled += Enemy_OnKill;
            m_EnemyBatch.EnemyReachedBottom += Enemy_OnReachBottom;
            m_EnemyBatch.NoMoreEnemies += enemy_OnDisposed;
            this.Add(m_EnemyBatch);

            WallBatch wallBatch = new WallBatch(this.Game);
            this.Add(wallBatch);

            MothershipEnemy mothershipEnemy = new MothershipEnemy(this.Game, ObjectValues.MothershipTextureString, ObjectValues.MothershipValue);
            mothershipEnemy.Position = new Vector2(0, ObjectValues.EnemyWidth);
            mothershipEnemy.MothershipKilled += Enemy_OnKill;
            this.Add(mothershipEnemy);

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
                    new UserSpaceship(this.Game, ObjectValues.SpaceShipTextures[textureIndex], playerId, startingPosition);
                spaceShip.Position = startingPosition;
                spaceShip.Shoot += spaceship_Shot;
                this.Add(spaceShip);

                player = new SpaceShipPlayer(spaceShip, playerId);
                player.PlayerHit += Player_OnHit;
                player.PlayerDead += Player_OnKilled;

                string scoreBoardText = "P" + (textureIndex + 1) + " Score: ";
                scoreBoard = new ScoreBoard(this.Game, scoreBoardText, ObjectValues.ConsolasFont);
                scoreBoard.Position = scorePosition;
                scoreBoard.Tint = ObjectValues.ScoreBoardsColors[textureIndex];
                player.ScoreBoard = scoreBoard;
                this.Add(scoreBoard);

                m_Players.Add(player); 
                textureIndex++;
                startingPosition.X += ObjectValues.SpaceshipSize;
                scorePosition.Y += 20;
            }

            SoulsBatch player1Souls = new SoulsBatch(this.Game, Color.White, new Vector2(GraphicsDevice.Viewport.Width - 80, 20));
            this.Add(player1Souls);
            SoulsBatch player2Souls = new SoulsBatch(this.Game, Color.ForestGreen, new Vector2(GraphicsDevice.Viewport.Width - 80, 45));
            this.Add(player2Souls);
            m_Players[0].SoulBatch = player1Souls;
            m_Players[1].SoulBatch = player2Souls;
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

        public override void Draw(GameTime gameTime)
        {
            
            base.Draw(gameTime);
        }

        private void spaceship_Shot(object i_Sender, EventArgs i_EventArgs)
        {
            SpaceBullet newBullet = new SpaceBullet(this.Game, ObjectValues.BulletTextureString, ObjectValues.UserShipBulletTint, -1);
            newBullet.Initialize();
            newBullet.Owner = i_Sender as UserSpaceship;
            setNewSpaceshipBulletPosition(i_Sender as UserSpaceship, newBullet);
            newBullet.Disposed += (i_Sender as UserSpaceship).OnMyBulletDisappear;
            newBullet.Disposed += onComponentDisposed;
            this.Add(newBullet);
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
            if (this.Count > 0)
            {
                this.Remove(i_Disposed as IGameComponent);
            }
        }

        public void Enemy_OnReachBottom(object i_ReacedBottomObj, EventArgs i_EventArgs)
        {
            GameOver();
        }
    }
}
