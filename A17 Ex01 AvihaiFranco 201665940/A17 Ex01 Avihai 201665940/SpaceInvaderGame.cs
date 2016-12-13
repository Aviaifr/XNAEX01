﻿using System;
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
        private List<IGameObject> m_MovingObjects;
        private float m_FixEnemyOffset;

        private int PointsCollected
        {
            get;
            set; 
        }

        public void Enemy_OnKill(object i_EnemyKilled, EventArgs i_eventArgs)
        {
            if (this.Components != null)
            {
                m_Player.Score += (i_EnemyKilled as Enemy).Value;
                this.Window.Title = m_Player.Score.ToString();
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
            GameOver();
        }

        public void GameOver()
        {
            System.Windows.Forms.MessageBox.Show("Final Score: " + m_Player.Score.ToString(),"Game Over!");
            this.Exit();
        }

        //public void Spaceship_onHit(int i_PointsToRemove)
        //{
        //    PointsCollected = (int)MathHelper.Clamp(PointsCollected - ObjectValues.SpaceshipValue, 0, int.MaxValue);
        //}

        public SpaceInvaderGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            m_MovingObjects = new List<IGameObject>();
            m_Background = new Background(this, ObjectValues.BackgroundTextureString);
            Components.Add(m_Background);

            UserSpaceship spaceship = new UserSpaceship(this, ObjectValues.UserShipTextureString);
            spaceship.Position = new Vector2(0, GraphicsDevice.Viewport.Height - ObjectValues.SpaceshipSize);
            spaceship.Shoot += spaceship_Shot;
            Components.Add(spaceship);
            m_Player = new SpaceShipPlayer(spaceship);
            m_Player.PlayerHit += Player_OnHit;
            m_Player.PlayerDead += Player_OnKilled;
            

            EnemyBatch enemyBatch = new EnemyBatch(this);
            enemyBatch.EnemyKilled += Enemy_OnKill;
            Components.Add(enemyBatch);

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
