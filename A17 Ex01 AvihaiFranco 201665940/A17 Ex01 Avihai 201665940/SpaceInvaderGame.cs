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
        private SpriteBatch m_SpriteBatch;
        private Background m_Background;
        private List<IGameObject> m_MovingObjects;
        private float m_FixEnemyOffset;
        private int PointsCollected { get; set; }

        public void Enemy_OnKill(object i_EnemyKilled, EventArgs i_eventArgs)
        {
            PointsCollected += (i_EnemyKilled as Enemy).Value;
            //this.Window.Title = PointsCollected.ToString();
        }

        public void Spaceship_onHit(int i_PointsToRemove)
        {
            PointsCollected = (int)MathHelper.Clamp(PointsCollected - ObjectValues.sr_SpaceshipValue,0, int.MaxValue);
        }

        public SpaceInvaderGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            System.Windows.Forms.MessageBox.Show("Test");
            m_MovingObjects = new List<IGameObject>();
            m_Background = new Background(this, ObjectValues.sr_Background);
            Components.Add(m_Background);

            UserSpaceship spaceship = new UserSpaceship(this, ObjectValues.sr_UserShip);
            spaceship.Position = new Vector2(0, GraphicsDevice.Viewport.Height - ObjectValues.sr_SpaceshipSize);
            spaceship.Shoot += spaceship_Shot;
            Components.Add(spaceship);

            EnemyBatch enemyBatch = new EnemyBatch(this);
            enemyBatch.EnemyKilled += Enemy_OnKill;
            Components.Add(enemyBatch);

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

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        private void enemyWallHitHandler(Sprite i_ObjectHitTheWall, float i_XFixOffset)
        {
            m_FixEnemyOffset = i_XFixOffset;
        }


        private void spaceship_Shot(object i_Sender,EventArgs i_EventArgs)
        {
            SpaceBullet newBullet = new SpaceBullet(this,ObjectValues.sr_Bullet,ObjectValues.sr_UserShipBulletTint);
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

        public void onComponentDisposed(object i_Disposed,EventArgs i_EventArgs)
        {
            Components.Remove(i_Disposed as IGameComponent);
        }
    }
}
