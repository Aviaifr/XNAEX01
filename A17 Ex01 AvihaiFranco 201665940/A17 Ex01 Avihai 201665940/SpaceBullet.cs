using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class SpaceBullet : Sprite, ICollidable2D
    {
        private static readonly Vector2 sr_BulletSpeed = new Vector2(0, 120);

        public SpaceBullet(Game i_Game, string i_TextureString, Color i_Tint)
            : base(i_Game, i_TextureString)
        {
            this.Tint = i_Tint;
            this.Velocity = sr_BulletSpeed;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_Position.Y += m_Speed.Y * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_Position.Y >= Game.GraphicsDevice.Viewport.Height || (m_Position.Y <= 0 && m_Speed.Y < 0))
            {
                onDisappeared();
            }

            OnPositionChanged();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void onDisappeared()
        {
            Dispose();
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Wall)
            {
                int i = 1;
            }
            bool shouldDispose = false;
            if (i_Collidable is SpaceBullet && this.m_Speed.Y > 0)
            {
                shouldDispose = s_RandomGen.Next(0, 2) == 0;
            }
            else
            {
                shouldDispose = true;
            }
            if (shouldDispose)
            {
                this.Dispose();
            }
        }

        public override bool CanCollideWith(ICollidable i_Source)
        {
            bool canCollide = true;
            if (i_Source is SpaceBullet)
            {
                canCollide = this.m_Speed.Y / Math.Abs(this.m_Speed.Y) != (i_Source as SpaceBullet).m_Speed.Y / Math.Abs((i_Source as SpaceBullet).m_Speed.Y);
            }
            return canCollide;
        }
    }
}
