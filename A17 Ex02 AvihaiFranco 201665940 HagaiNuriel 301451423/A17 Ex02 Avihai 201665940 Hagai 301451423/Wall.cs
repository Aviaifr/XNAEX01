using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Animators;

namespace Space_Invaders
{
    public class Wall : Sprite, ICollidable2D
    {
        private Rectangle m_RectangleToErase;
        
        public Wall(Game i_Game, string i_TextureString)
            : base(i_Game, i_TextureString)
        {
            m_TintColor = Color.White;
        }
        
        public void Collided(ICollidable i_Collidable)
        {
            (Game.Services.GetService(typeof(ISoundEffectsPlayer)) as ISoundEffectsPlayer).PlaySoundEffect(GetSound("hit"));
            for (int i = 0; i < m_TextureColorData.Length; i++)
            {
                Point indexToPoint = new Point((int)m_Position.X + (i % m_Texture.Width), (int)m_Position.Y + (i / m_Texture.Width));
                if (m_RectangleToErase.Contains(indexToPoint))
                {
                    m_TextureColorData[i].A = 0;
                }
            }
        }

        public override void Initialize()
        {
            m_Sounds.Add("hit", Game.Content.Load<SoundEffect>(@"C:/Temp/XNA_Assets/Ex03/Sounds/BarrierHit"));
            base.Initialize();
        }

        public override bool IsPixelBasedCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            int collidierHeight = source.Bounds.Height;
            int directionFactor = (int)((i_Source as Sprite).Velocity.Y / Math.Abs((i_Source as Sprite).Velocity.Y));
            for (int i = 0; i < m_TextureColorData.Length && !collided; i++)
            {
                if (m_TextureColorData[i].A != 0)
                {
                    Point CollidablePointOnScreen = new Point((int)m_Position.X + (i % m_Texture.Width), (int)m_Position.Y + (i / m_Texture.Width));
                    if (source.IsPointInScreenIsColidablePixel(CollidablePointOnScreen))
                    {
                        m_RectangleToErase = source.Bounds;
                        if (i_Source is SpaceBullet)
                        {
                            m_RectangleToErase.Y += directionFactor * (int)(0.45f * collidierHeight);
                        }

                        collided = true;
                    }
                }
            }

            return collided;
        }

        public override void Draw(GameTime gameTime)
        {
            this.m_Texture.SetData(m_TextureColorData);
            base.Draw(gameTime);
        }

        public override bool CanCollideWith(ICollidable i_Source)
        {
            bool canCollide = true;

            return canCollide;
        }

        protected override void setupAnimations()
        {
            this.Animations.Enabled = true;
        }
    }
}
