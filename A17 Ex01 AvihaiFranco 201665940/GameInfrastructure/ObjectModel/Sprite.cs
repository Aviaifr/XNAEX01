using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.ServiceInterfaces;

namespace GameInfrastructure.ObjectModel
{
    public abstract class Sprite : DynamicDrawableComponent
    {
        protected Vector2 m_Position;
        protected Texture2D m_Texture;
        protected Color m_Tint;
        protected Game m_Game;
        protected Vector2 m_Speed;
        public static Random s_RandomGen = new Random();
        protected int m_Width;
        protected int m_Height;
        private bool m_UseSharedBatch = false;
        protected SpriteBatch m_SpriteBatch;
        protected Color[] m_TextureColorData;
        public SpriteBatch SpriteBatch
        {
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        public Sprite(Game i_Game, string i_TextureString) : base(i_TextureString, i_Game)
        {
        }

        public Sprite(Game i_Game) : base(i_Game)
        {
        }

        public Sprite(Game i_Game, string i_TesxtureString, int i_CallsOrder) : base(i_TesxtureString, i_Game, i_CallsOrder)
        {
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Vector2 Velocity
        {
            get { return m_Speed; }
            set { m_Speed = value; }
        }

        public Color Tint
        {
            get { return m_Tint; }
            set { m_Tint = value; }
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
            m_TextureColorData = new Color[m_Texture.Width * m_Texture.Height];
            m_Texture.GetData(m_TextureColorData);
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)m_Position.X,
                    (int)m_Position.Y,
                    m_Width,
                    m_Height);
            }

            set
            {
                Bounds = value;
            }
        }

        public virtual bool IsCollidedWith(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            if (source != null)
            {
                if (CanCollideWith(i_Source) && (source.Bounds.Intersects(this.Bounds) || source.Bounds.Contains(this.Bounds)))
                {
                    collided = IsPixelBasedCollision(i_Source);
                }
            }
            return collided;
        }

        public virtual bool IsPixelBasedCollision(ICollidable i_Source)
        {
            bool notFound = true;
            ICollidable2D source = i_Source as ICollidable2D;
            for (int i = 0; i < m_TextureColorData.Length && notFound; i++)
            {
                if (m_TextureColorData[i].A != 0)
                {
                    Point CollidablePointOnScreen = new Point((int)m_Position.X + (i % m_Texture.Width), (int)m_Position.Y + i / m_Texture.Width);
                    notFound = !source.IsPointInScreenIsColidablePixel(CollidablePointOnScreen);
                }
            }

            return !notFound;
        }

        public virtual bool IsPointInScreenIsColidablePixel(Point i_PointOnScreen)
        {
            bool result = false;
            if(this.Bounds.Contains(i_PointOnScreen))
            {
                int xDifference = i_PointOnScreen.X - (int)m_Position.X;
                int yDifference = i_PointOnScreen.Y - (int)m_Position.Y;
                result = m_TextureColorData[yDifference * m_Texture.Width + xDifference].A != 0;
            }

            return result;
        }

        public virtual void Collided(ICollidable i_Collidable)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.IsVisible)
            {
                if (this.m_UseSharedBatch == false)
                {
                    SpriteBatch spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                    spriteBatch.Draw(m_Texture, m_Position, m_Tint);
                    spriteBatch.End();
                }
                else
                {
                    m_SpriteBatch.Draw(m_Texture, m_Position, m_Tint);
                }
                
            }
        }

        public int Width
        {
            get { return m_Width; }
            set
            {
                if (m_Width != value)
                {
                    m_Width = value;
                    OnSizeChanged();
                }
            }
        }

        public int Height
        {
            get { return m_Height; }
            set
            {
                if (m_Height != value)
                {
                    m_Height = value;
                    OnSizeChanged();
                }
            }
        }

        protected override void InitBounds()
        {
            m_Width = m_Texture.Width;
            m_Height = m_Texture.Height;
        }

        public virtual bool CanCollideWith(ICollidable i_Source) { return true; }
    }
}
