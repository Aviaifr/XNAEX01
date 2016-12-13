using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class Enemy : Sprite, IShootingObject, ICollidable2D
    {
        public int Value { get; set; }

        public event EventHandler<EventArgs> Shoot;

        protected float m_timeSinceMoved;
        protected float m_TimeBetweenJumps;
        protected static int s_fireChance = 1;
        protected float m_Direction = 1;
        protected int m_NumOfJumps;

        public bool WasHit { get; set; }

        public Enemy(Game i_Game, string i_TextureLocation, int i_Value)
            : base(i_Game, i_TextureLocation)
        {
            Value = i_Value;
            WasHit = false;
        }

        public float Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        public float TimeBetweenJumps
        {
            get { return m_TimeBetweenJumps; }
        }

        public float TimeSinceMoved
        {
            get { return m_timeSinceMoved; }
            set { m_timeSinceMoved = value; }
        }

        public int NumOfJumps
        {
            get { return m_NumOfJumps; }
            set { m_NumOfJumps = value; }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_timeSinceMoved = 0;
            m_Speed.X = m_Texture.Width / 2;
            m_TimeBetweenJumps = 0.5f;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_Position += m_Speed * m_NumOfJumps;
            tryToShoot();
            OnPositionChanged();
        }

        protected bool hitWall()
        {
            return (m_Position.X + m_Texture.Width >= Game.GraphicsDevice.Viewport.Width && m_Speed.X > 0)
                || (m_Position.X <= 0 && m_Speed.X < 0);
        }

        protected virtual void tryToShoot()
        {
            int randNumToFire = s_RandomGen.Next(0, 100);
            if (randNumToFire < s_fireChance)
            {
                OnShoot();
            }
        }

        private void OnShoot()
        {
            if (this.Shoot != null)
            {
                Shoot.Invoke(this, EventArgs.Empty);
            }
        }

        public Vector2 GetShotStartingPosition()
        {
            float x, y;
            x = m_Position.X + (m_Texture.Width / 2);
            y = m_Position.Y + m_Texture.Height;
            return new Vector2(x, y);
        }

        public void ChangeDirection()
        {
            m_Speed.X *= -1;
        }

        public void OnMyBulletDisappear(object i_SpaceBullet, EventArgs i_EventArgs)
        {
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is SpaceBullet)
            {
                if ((i_Collidable as SpaceBullet).Velocity.Y < 0)
                {
                    this.Dispose();
                }
            }
        }
    }
}
