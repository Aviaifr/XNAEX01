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
                if(i_EnemyKilled is MothershipEnemy)
                {
                    MothershipEnemy motherShip = i_EnemyKilled as MothershipEnemy;
                    motherShip.Velocity = Vector2.Zero;
                    motherShip.Animations.Enable(ObjectValues.sr_DeathAnimation);
                    motherShip.Animations.Reset(ObjectValues.sr_DeathAnimation); 
                }
                checkWin();
            }
        }

        private void checkWin()
        {
            if (m_EnemyBatch.EnemyCount == 0)
            {
                GameOver(eGameOverType.PlayerWins);
            }
        }

        public void Player_OnHit(object i_HitPlayer, EventArgs i_EventArgs)
        {
            resetPlayerSpaceShipPosition(i_HitPlayer as SpaceShipPlayer);
        }

        private void resetPlayerSpaceShipPosition(SpaceShipPlayer i_Player)
        {
            //i_Player.GameComponentPosition = new Vector2
              //  (0, GraphicsDevice.Viewport.Height - m_Player.GameComponentBounds.Height);
            i_Player.GameComponenetAnimations.Reset(ObjectValues.sr_HitAnimation);
            i_Player.GameComponenetAnimations.Enable(ObjectValues.sr_HitAnimation);

        }

        public void Player_OnKilled(object i_HitPlayer, EventArgs i_EventArgs)
        {
            //GameOver(eGameOverType.GameOver);
            (i_HitPlayer as SpaceShipPlayer).GameComponenetAnimations.Enable(ObjectValues.sr_DeathAnimation);

        }

        public void GameOver(eGameOverType i_GameOverType)
        {
            string msgTitle = string.Empty;
            switch (i_GameOverType)
            {
                case eGameOverType.GameOver:
                    msgTitle = "Game Over!";
                    break;
                case eGameOverType.PlayerWins:
                    msgTitle = "Player Wins!!";
                    break;
            }

            System.Windows.Forms.MessageBox.Show(string.Format("Final Score: {0}", m_Player.Score.ToString()), msgTitle);
            this.Exit();
        }

        public SpaceInvaderGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.Window.Title = "Space Invaders";
            m_Background = new Background(this, ObjectValues.BackgroundTextureString);
            Components.Add(m_Background);

            UserSpaceship spaceship = new UserSpaceship(this, ObjectValues.UserShipTextureString);
            spaceship.Position = new Vector2(0, GraphicsDevice.Viewport.Height - ObjectValues.SpaceshipSize);
            spaceship.Shoot += spaceship_Shot;
            Components.Add(spaceship);
            m_Player = new SpaceShipPlayer(spaceship);
            m_Player.PlayerHit += Player_OnHit;
            m_Player.PlayerDead += Player_OnKilled;            

            m_EnemyBatch = new EnemyBatch(this);
            m_EnemyBatch.EnemyKilled += Enemy_OnKill;
            Components.Add(m_EnemyBatch);

            WallBatch w = new WallBatch(this,400);
            Components.Add(w);

            MothershipEnemy mothershipEnemy = new MothershipEnemy(this, ObjectValues.MothershipTextureString, ObjectValues.MothershipValue);
            mothershipEnemy.Position = new Vector2(0, ObjectValues.EnemyWidth);
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
            SpaceBullet newBullet = new SpaceBullet(this, ObjectValues.BulletTextureString, ObjectValues.UserShipBulletTint);
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
