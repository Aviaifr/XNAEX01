using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public abstract class SpaceObject : IGameObject
    {
        protected Vector2 m_Position;
        protected Texture2D m_Texture;
        protected Color m_Tint;
        protected Game m_Game;
        protected Vector2 m_Speed;
        protected string m_TextureString;
        public static Random s_RandomGen = new Random();

        public SpaceObject(Game i_Game, string i_TextureString)
        {
            m_Game = i_Game;
            m_TextureString = i_TextureString;
        }

        public virtual void Initialize(Vector2 i_Position, Color i_Tint, Vector2 i_Speed)
        {
            m_Position = i_Position;
            m_Tint = i_Tint;
            m_Speed = i_Speed;
            LoadContent();
        }

        protected void LoadContent()
        {
            m_Texture = m_Game.Content.Load<Texture2D>(m_TextureString);
        }
        
        public abstract void Update(GameTime i_GameTime);
        
        public void Draw(SpriteBatch i_SpriteBatch)
        {
            i_SpriteBatch.Draw(m_Texture, m_Position, m_Tint);
        }
    }
}
