using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public delegate void WallHitHandler(SpaceObject i_ObjectHitTheWall, float i_OffsetFromHit);

    public class Enemy : SpaceObject, IShootingObject
    {
        public int Value { get; set; }

        public event WallHitHandler OnWallHit;
        public event ObjectFireHandler OnFire;

        private float m_timeSinceMoved;
        private float m_TimeBetweenJumps;
        private static int s_fireChance = 2;

        public Enemy(Game i_Game, string i_TextureLocation, int i_Value)
            : base(i_Game, i_TextureLocation)
        {
            Value = i_Value;
        }

        public override void Initialize(Vector2 i_Position, Color i_Tint, Vector2 i_Speed)
        {
            m_timeSinceMoved = 0;
            base.Initialize(i_Position, i_Tint, i_Speed);
            m_Speed.X = m_Texture.Width;
            m_TimeBetweenJumps = 0.5f;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_timeSinceMoved += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_timeSinceMoved >= m_TimeBetweenJumps)
            {
                m_Position.X += m_Speed.X * m_timeSinceMoved;
                m_timeSinceMoved = m_timeSinceMoved - m_TimeBetweenJumps;
            }

            if ((m_Position.X + m_Texture.Width >= m_Game.GraphicsDevice.Viewport.Width && m_Speed.X > 0) || (m_Position.X <= 0 && m_Speed.X < 0))
            {
                if (OnWallHit != null)
                {
                    float offset = (m_Speed.X > 0) ? m_Game.GraphicsDevice.Viewport.Width - (m_Position.X + m_Texture.Width)  : -m_Position.X;
                    OnWallHit.Invoke(this, offset);
                }
            }
            tryToShoot();
        }

        private void tryToShoot()
        {
            int randNumToFire = s_RandomGen.Next(0, 1000);
            if (randNumToFire < s_fireChance)
            {
                Shoot();
            }
        }

        public void Shoot()
        {
            if (OnFire != null)
            {
                OnFire.Invoke(this);
            }
        }

        public Vector2 GetShotStartingPosition()
        {
            float x, y;
            x = m_Position.X + (m_Texture.Width / 2);
            y = m_Position.Y + (m_Texture.Height);
            return new Vector2(x, y);
        }

        public void ChangeDirection(float i_MovementXOffset)
        {
            m_Speed.X *= -1;
            m_Position.X += i_MovementXOffset;
            m_Position.Y += m_Texture.Width / 2;
            m_TimeBetweenJumps = m_TimeBetweenJumps - (m_TimeBetweenJumps * 0.04f);
        }

        public void OnMybulletDisappear()
        {
        }
    }
}
