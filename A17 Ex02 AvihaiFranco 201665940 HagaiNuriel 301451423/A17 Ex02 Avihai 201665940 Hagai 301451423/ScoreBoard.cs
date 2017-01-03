using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    public class ScoreBoard : DrawableGameComponent
    {
        private string m_Text;
        private string m_SpriteFontLocation;
        private SpriteFont m_SpriteFont;
        private Game m_Game;
        private Vector2 m_Position;
        private int m_ScoreValue;
        private Color m_Tint;

        public ScoreBoard(Game i_Game, string i_Text, string i_SpriteFontLocation)
            : base(i_Game)
        {
            m_Game = i_Game;
            m_Text = i_Text;
            m_SpriteFontLocation = i_SpriteFontLocation;
        }

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

        public int ScoreValue
        {
            get { return m_ScoreValue; }
            set { m_ScoreValue = value; }
        }

        protected override void LoadContent()
        {
            m_SpriteFont = m_Game.Content.Load<SpriteFont>(m_SpriteFontLocation);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = 
                m_Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.DrawString(m_SpriteFont, m_Text + m_ScoreValue.ToString(), m_Position, m_Tint);
            spriteBatch.End();
        }
    }
}
