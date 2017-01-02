﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class EnemyBatch : Sprite
    {
        private readonly int r_BatchColumns = 9;
        private readonly int r_BatchRows = 5;
        private readonly int r_VerticalPadding = 32 * 3;
        private readonly float r_VerticalJumpLength = 32 * 0.5f;
        private readonly string r_Enemy1 = "Enemies/Enemy0101_32x32";
        private readonly string r_Enemy2 = "Enemies/Enemy0201_32x32";
        private readonly string r_Enemy3 = "Enemies/Enemy0301_32x32";
        private readonly Color r_Enemy1Tint = Color.LightPink;
        private readonly Color r_Enemy2Tint = Color.LightBlue;
        private readonly Color r_Enemy3Tint = Color.LightYellow;
        private readonly Color r_EnemyBulletTint = Color.Blue;
        private readonly int r_Enemy1Value = 240;
        private readonly int r_Enemy2Value = 170;
        private readonly int r_Enemy3Value = 140;
        private readonly float r_EnemySize = 32;
        private readonly Color r_EnemyButtletTint = Color.Blue;

        public event EventHandler<EventArgs> EnemyKilled;

        private List<Enemy> m_Enemies;
        private bool m_EnemyHitWall;
        private float m_XMax, m_XMin;
        private bool m_BatchMovingRight = true;
        private float m_TimeSinceMoved = 0f;
        private float m_TimeBetweenJumps = 0.5f;

        public int EnemyCount
        { 
            get
            {
                return this.m_Enemies.Count - m_Enemies.Count<Enemy>(enemy => enemy.WasHit == true);
            }
        }

        public EnemyBatch(Game i_Game) : base(i_Game)
        {
            m_Enemies = new List<Enemy>();
        }

        protected override void LoadContent()
        {
        }

        public override void Initialize()
        {
            for(int i = 0; i < r_BatchRows; i++)
            {
                for(int j = 0; j < r_BatchColumns; j++)
                {
                    float x = (float)(j * (1.6 * r_EnemySize));
                    float y = (float)(r_VerticalPadding + (i * 1.6 * r_EnemySize));
                    Enemy newEnemy = new Enemy(Game, GetEnemySpriteByRow(i), GetEnemyValueByRow(i));
                    newEnemy.Tint = GetEnemyTintByRow(i);
                    newEnemy.Position = new Vector2(x, y);
                    newEnemy.Shoot += enemy_OnShoot;
                    newEnemy.Disposed += onComponentDisposed;
                    newEnemy.Initialize();
                    m_Enemies.Add(newEnemy);
                }
            }
        }

        private void enemy_OnShoot(object i_Sender, EventArgs i_EventArgs)
        {
            SpaceBullet newBullet = new SpaceBullet
                (this.Game, ObjectValues.BulletTextureString, r_EnemyBulletTint);
            newBullet.Initialize();
            newBullet.Disposed += onComponentDisposed;
            setNewEnemyBulletPosition(i_Sender as Enemy, newBullet);
            this.Game.Components.Add(newBullet);
        }

        public void onComponentDisposed(object i_Disposed, EventArgs i_EventArgs)
        {
            if (m_Enemies.Contains(i_Disposed))
            {
                Enemy enemy = i_Disposed as Enemy;
                if (enemy.isCollidable)
                {
                    (i_Disposed as Enemy).isCollidable = false;
                    (i_Disposed as Enemy).Animations.Enabled = true;
                    speedUpEnemies();
                }
                if (EnemyKilled != null)
                {
                    EnemyKilled(i_Disposed, i_EventArgs);
                }
            }
            else if (this.Game.Components != null)
            {
                this.Game.Components.Remove(i_Disposed as IGameComponent);
            }
        }

        private void setNewEnemyBulletPosition(Enemy i_EnemyShot, SpaceBullet i_newBullet)
        {
            Vector2 newBulletPos = i_EnemyShot.Position;
            newBulletPos.X += i_EnemyShot.Width / 2;
            newBulletPos.X -= i_newBullet.Width / 2;
            newBulletPos.Y += i_EnemyShot.Height;
            i_newBullet.Position = newBulletPos;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_TimeSinceMoved += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimeSinceMoved >= m_TimeBetweenJumps)
            {
                if (m_EnemyHitWall)
                {
                    m_EnemyHitWall = false;
                    speedUpEnemies();
                    foreach (Enemy enemy in m_Enemies)
                    {
                        Vector2 newPosition = enemy.Position;
                        newPosition.Y += r_VerticalJumpLength;
                        enemy.Position = newPosition;
                        enemy.ChangeDirection();
                    }
                }
                else
                {
                    m_XMax = 0;
                    m_XMin = Game.GraphicsDevice.Viewport.Width;
                    foreach (Enemy enemy in m_Enemies)
                    {
                        enemy.NumOfJumps = (int)(m_TimeSinceMoved / m_TimeBetweenJumps);
                        enemy.Update(i_GameTime);

                        if (m_BatchMovingRight)
                        {
                            if (enemy.Position.X > m_XMax)
                            {
                                m_XMax = enemy.Position.X;
                            }
                        }
                        else
                        {
                            if (enemy.Position.X < m_XMin)
                            {
                                m_XMin = enemy.Position.X;
                            }
                        }
                    }

                    if (batchHitWall())
                    {
                        float offset = getOffset();            
                        readjustEnemyBatch(offset);
                        m_BatchMovingRight = !m_BatchMovingRight;
                        m_EnemyHitWall = true;
                    }
                }

                m_TimeSinceMoved -= m_TimeBetweenJumps;
            }
            else
            {
                updateAnimations(i_GameTime);
            }

            m_Enemies.RemoveAll(enemy => enemy.WasHit == true);
        }

        private void updateAnimations(GameTime i_GameTime)
        {
            foreach(Enemy enemy in m_Enemies)
            {
                enemy.Animations.Update(i_GameTime);
            }
        }

        private void speedUpEnemies()
        {
            m_TimeBetweenJumps -= m_TimeBetweenJumps * 0.04f;
        }

        private float getOffset()
        {
            float offset;
            if (m_BatchMovingRight)
            {
                offset = m_XMax - (Game.GraphicsDevice.Viewport.Width - r_EnemySize);
            }
            else
            {
                offset = m_XMin;
            }

            return offset;
        }

        private void readjustEnemyBatch(float offset)
        {
            foreach(Enemy enemy in m_Enemies)
            {
                enemy.Position = new Vector2(enemy.Position.X - offset, enemy.Position.Y);
            }
        }

        private bool batchHitWall()
        {
            return (m_XMax >= (Game.GraphicsDevice.Viewport.Width - r_EnemySize) && m_BatchMovingRight) || (m_XMin <= 0 && !m_BatchMovingRight);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(Enemy enemy in m_Enemies)
            {
                enemy.Draw(gameTime);
            }
        }

        private string GetEnemySpriteByRow(int i_Row)
        {
            string spriteLoc = string.Empty;
            switch (i_Row)
            {
                case 0:
                    spriteLoc = r_Enemy1;
                    break;
                case 1:
                case 2:
                    spriteLoc = r_Enemy2;
                    break;
                case 3:
                case 4:
                    spriteLoc = r_Enemy3;
                    break;
            }

            return spriteLoc;
        }

        private Color GetEnemyTintByRow(int i_Row)
        {
            Color enemyTint = Color.White;
            switch (i_Row)
            {
                case 0:
                    enemyTint = r_Enemy1Tint;
                    break;
                case 1:
                case 2:
                    enemyTint = r_Enemy2Tint;
                    break;
                case 3:
                case 4:
                    enemyTint = r_Enemy3Tint;
                    break;
            }

            return enemyTint;
        }

        private int GetEnemyValueByRow(int i_Row)
        {
            int enemyVal = 0;
            switch (i_Row)
            {
                case 0:
                    enemyVal = r_Enemy1Value;
                    break;
                case 1:
                case 2:
                    enemyVal = r_Enemy2Value;
                    break;
                case 3:
                case 4:
                    enemyVal = r_Enemy3Value;
                    break;
            }

            return enemyVal;
        }
    }
}
