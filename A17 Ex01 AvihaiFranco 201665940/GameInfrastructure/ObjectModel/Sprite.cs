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
        protected string m_TextureString;
        public static Random s_RandomGen = new Random();

        public Sprite(Game i_Game, string i_TextureString) : base(i_TextureString, i_Game)
        {
            //m_TextureString = i_TextureString;
        }

        public Sprite(Game i_Game) : base(i_Game) { }

        public Sprite(Game i_Game, string i_TesxtureString, int i_CallsOrder): base(i_TesxtureString, i_Game, i_CallsOrder)
        { }

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Color Tint
        {
            get { return m_Tint; }
            set { m_Tint = value; }
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
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
        }

        public virtual bool IsCollidedWith(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            if (source != null)
            {
                collided = source.Bounds.Intersects(this.Bounds) || source.Bounds.Contains(this.Bounds);
            }

            return collided;
        }
        // -- end of TODO 14

        // TODO 15: Implement a basic collision reaction between two ICollidable2D objects
        public virtual void Collided(ICollidable i_Collidable)
        {
        }

        //public override void Initialize()
        //{
        //    base.Initialize();
        //}

        //public virtual void Initialize(Vector2 i_Position, Color i_Tint, Vector2 i_Speed)
        //{
        //    m_Position = i_Position;
        //    m_Tint = i_Tint;
        //    m_Speed = i_Speed;
        //    LoadContent();
        //}

        //protected void LoadContent()
        //{
        //    m_Texture = m_Game.Content.Load<Texture2D>(m_TextureString);
        //}

        //public abstract void Update(GameTime i_GameTime);

        //public void Draw(SpriteBatch i_SpriteBatch)
        //{
        //    i_SpriteBatch.Draw(m_Texture, m_Position, m_Tint);
        //}

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            //SpriteBatch spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            spriteBatch.Begin();
            spriteBatch.Draw(m_Texture, m_Position, m_Tint);
            spriteBatch.End();
        }

        protected int m_Width;
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

        protected int m_Height;
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
            //m_Position = Vector2.Zero; TODO: why was this here?
        }

    }
}
