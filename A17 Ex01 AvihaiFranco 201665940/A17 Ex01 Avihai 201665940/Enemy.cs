using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace A17_Ex01_Avihai_201665940
{
    public delegate void WallHitHandler(Sprite i_ObjectHitTheWall, float i_OffsetFromHit);

    public class Enemy : Sprite, IShootingObject
    {
        public int Value { get; set; }

        public event WallHitHandler OnWallHit;

        public event ObjectFireHandler OnFire;

        private float m_timeSinceMoved;
        private float m_TimeBetweenJumps;
        private static int s_fireChance = 2;
        private float m_Direction = 1;
        private int m_NumOfJumps;
        public Enemy(Game i_Game, string i_TextureLocation, int i_Value)
            : base(i_Game, i_TextureLocation)
        {
            Value = i_Value;
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
            get { return m_NumOfJumps;}
            set { m_NumOfJumps = value; }
        }
        public override void Initialize()
        {
            base.Initialize();
            m_timeSinceMoved = 0;
            //base.Initialize(i_Position, i_Tint, i_Speed); TODO: figure this out
            m_Speed.X = m_Texture.Width / 2;
            m_TimeBetweenJumps = 0.5f;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_Position.X += (m_Speed.X * m_NumOfJumps);
            tryToShoot();
        }

        private bool hitWall()
        {
            return (m_Position.X + m_Texture.Width >= Game.GraphicsDevice.Viewport.Width && m_Speed.X > 0)
                || (m_Position.X <= 0 && m_Speed.X < 0);
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
           // SpaceBullet bullet = new SpaceBullet(Game,ObjectValues.)
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

        public void OnMybulletDisappear(SpaceBullet i_DisappearedBullet)
        {
        }
    }
}
