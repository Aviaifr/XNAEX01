using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Animators;

namespace GameInfrastructure.ObjectModel
{
    public abstract class CompositeSprite : Sprite
    {
        protected readonly List<Sprite> m_SpritesList = new List<Sprite>();
        
        public CompositeSprite(Game i_Game)
            : base(i_Game)
        {
        }

        public virtual void Add(Sprite i_spriteToAdd)
        {
            m_SpritesList.Add(i_spriteToAdd);
        }
         
        public override void Draw(GameTime gameTime)
        {
            foreach (Sprite sprite in m_SpritesList)
            {
                sprite.Draw(gameTime);
            }
        }

        public override void Initialize()
        {
            foreach (Sprite sprite in m_SpritesList)
            {
                sprite.Initialize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach(Sprite sprite in m_SpritesList)
            {
                sprite.Update(gameTime);
            }
        }

        public virtual void Empty()
        {
            foreach(Sprite sprite in m_SpritesList)
            {
                sprite.Dispose();
            }

            m_SpritesList.Clear();
        }

        public override bool IsCollidedWith(ServiceInterfaces.ICollidable i_Source)
        {
            return false;
        }

        public override bool IsPixelBasedCollision(ICollidable i_Source)
        {
            return false;
        }

        public override bool IsPointInScreenIsColidablePixel(Microsoft.Xna.Framework.Point i_PointOnScreen)
        {
            return false;
        }
        
        public override bool CanCollideWith(ServiceInterfaces.ICollidable i_Source)
        {
            return false;
        }
    }
}
