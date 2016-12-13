using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace A17_Ex01_Avihai_201665940
{
    public class SpaceInvaderGame : Game
    {
        public static int EnemyRows = 5;
        public static int EnemyCols = 9;
        private GraphicsDeviceManager m_Graphics;
        private SpaceShipPlayer m_Player;
        private SpriteBatch m_SpriteBatch;
        private Background m_Background;
        private EnemyBatch m_EnemyBatch;
        private int PointsCollected
        {
            get;
            set; 
        }

        public enum eGameOverType
        {
            GameOver,
            PlayerWins
        }

        public void Enemy_OnKill(object i_EnemyKilled, EventArgs i_eventArgs)
        {
            if (this.Components != null)
            {
                m_Player.Score += (i_EnemyKilled as Enemy).Value;
                checkWin();
            }
        }

        private void checkWin()
        {
            this.Window.Title = m_EnemyBatch.EnemyCount.ToString();
            if (m_EnemyBatch.EnemyCount == 0)
            {
                GameOver(eGameOverType.PlayerWins);
            }
        }

        public void Player_OnHit(object i_HitPlayer, EventArgs i_EventArgs)
        {
            resetPlayerSpaceShipPosition();
            if (this.Window != null)
            {
                this.Window.Title = m_Player.Score.ToString();
            }
        }

        private void resetPlayerSpaceShipPosition()
        {
            m_Player.GameComponentPosition = new Vector2
                (0, GraphicsDevice.Viewport.Height - m_Player.GameComponentBounds.Height);
        }

        public void Player_OnKilled(object i_HitPlayer, EventArgs i_EventArgs)
        {
            GameOver(eGameOverType.GameOver);
        }

        public void GameOver(eGameOverType i_GameOverType)
        {
            string msgTitle = String.Empty;
            switch (i_GameOverType)
            {
                case eGameOverType.GameOver:
                    msgTitle = "Game Over!";
                    break;
                case eGameOverType.PlayerWins:
                    msgTitle = "Player Wins!!";
                    break;
            }
            System.Windows.Forms.MessageBox.Show(String.Format("Final Score: {0}", m_Player.Score.ToString()),msgTitle);
            this.Exit();
        }

        public SpaceInvaderGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            m_Background = new Background(this, ObjectValues.sr_BackgroundTextureString);
            Components.Add(m_Background);

            UserSpaceship spaceship = new UserSpaceship(this, ObjectValues.sr_UserShipTextureString);
            spaceship.Position = new Vector2(0, GraphicsDevice.Viewport.Height - ObjectValues.sr_SpaceshipSize);
            spaceship.Shoot += spaceship_Shot;
            Components.Add(spaceship);
            m_Player = new SpaceShipPlayer(spaceship);
            m_Player.PlayerHit += Player_OnHit;
            m_Player.PlayerDead += Player_OnKilled;            

            m_EnemyBatch = new EnemyBatch(this);
            m_EnemyBatch.EnemyKilled += Enemy_OnKill;
            Components.Add(m_EnemyBatch);

            MothershipEnemy mothershipEnemy = new MothershipEnemy(this, ObjectValues.sr_MothershipTextureString, ObjectValues.sr_MothershipValue);
            mothershipEnemy.Position = new Vector2(0, ObjectValues.sr_EnemyWidth);
            mothershipEnemy.MothershipKilled += Enemy_OnKill;
            Components.Add(mothershipEnemy);

            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);
            new InputManager(this);
            new CollisionsManager(this);
            base.Initialize();
        }

        private Vector2 getEnemyPosition(int i_row, int i_col)
        {
            float y = (3 * ObjectValues.sr_EnemyWidth) + ((1.6f * ObjectValues.sr_EnemyWidth) * i_row);
            float x = (1.6f * ObjectValues.sr_EnemyWidth) * i_col;
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
            SpaceBullet newBullet = new SpaceBullet(this, ObjectValues.sr_BulletTextureString, ObjectValues.sr_UserShipBulletTint);
            newBullet.Initialize();
            setNewSpaceshipBulletPosition(i_Sender as UserSpaceship, newBullet);
            newBullet.Disposed += (i_Sender as UserSpaceship).OnMyBulletDisappear;
            newBullet.Disposed += onComponentDisposed;
            newBullet.Velocity *= -1; 
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
    }
}
