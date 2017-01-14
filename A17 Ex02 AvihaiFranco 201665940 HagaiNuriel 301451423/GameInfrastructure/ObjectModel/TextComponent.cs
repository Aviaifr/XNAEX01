using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameInfrastructure.ObjectModel
{
    public class TextComponent: DynamicDrawableComponent
    {
        protected SpriteFont m_SpriteFont;
        protected string m_SpriteFontLocation;
        protected string m_ExtraText;
        public float Size { get; set; }
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public virtual Color Tint { get; set; }
        public string ExtraText { get { return m_ExtraText == string.Empty ? m_ExtraText : " : " + m_ExtraText; } set { m_ExtraText = value; } }
        
        public TextComponent(Game i_Game, string i_Text, string i_SpriteFontLocation)
            : base(i_Game)
        {
            m_ExtraText = string.Empty;
            Text = i_Text;
            m_SpriteFontLocation = i_SpriteFontLocation;
            Size = 2f;
        }

        protected override void LoadContent()
        {
            m_SpriteFont = this.Game.Content.Load<SpriteFont>(m_SpriteFontLocation);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch =
                this.Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.DrawString(m_SpriteFont, Text + ExtraText, Position, Tint,0,Vector2.Zero,Size,SpriteEffects.None,0);
            spriteBatch.End();
        }

        protected override void InitBounds()
        {
        }
    }
}
