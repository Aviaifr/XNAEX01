using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class Wall : Sprite, ICollidable2D
    {
        private List<int> m_indexesToDelete;
        
        public Wall(Game i_Game, string i_TextureString)
            : base(i_Game, i_TextureString)
        {
            m_Tint = Color.White;
        }

        public override void Initialize()
        {
            m_indexesToDelete = new List<int>();
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        
        public override void Collided(ICollidable i_Collidable)
        {
            foreach (int index in m_indexesToDelete)
            {
                m_TextureColorData[index].A = 0;
            }

            this.m_Texture.SetData(m_TextureColorData);
            m_indexesToDelete.Clear();
        }

        public override bool IsPixelBasedCollision(ICollidable i_Source)
        {
            bool collided = false;
            float collidingHeight = (i_Source as ICollidable2D).Bounds.Height * 0.45f;
            float directionFactor = (i_Source as Sprite).Velocity.Y / Math.Abs((i_Source as Sprite).Velocity.Y);
            ICollidable2D source = i_Source as ICollidable2D;
            for (int i = 0; i < m_TextureColorData.Length; i++)
            {
                if (m_TextureColorData[i].A != 0)
                {
                    Point CollidablePointOnScreen = new Point((int)m_Position.X + (i % m_Texture.Width), (int)m_Position.Y + i / m_Texture.Width);
                    if (source.IsPointInScreenIsColidablePixel(CollidablePointOnScreen))
                    {
                        if (i_Source is SpaceBullet)
                        {
                            for (int j = 0; j < collidingHeight; j++)
                            {
                                int pixToDeletPos = i + (int)(j * directionFactor * this.Width);
                                if (pixToDeletPos >= 0 && pixToDeletPos < m_TextureColorData.Length)
                                {
                                    m_indexesToDelete.Add(pixToDeletPos);
                                }
                            }
                        }
                        else
                        {
                            m_indexesToDelete.Add(i);
                        }

                        collided = true;
                    }
                }
            }

            return collided;
        }

        public override bool CanCollideWith(ICollidable i_Source)
        {
            bool canCollide = true;

            return canCollide;
        }
    }
}
