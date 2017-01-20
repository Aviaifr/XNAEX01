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
    public class TextComponent : DynamicDrawableComponent
    {
        protected SpriteFont m_SpriteFont;
        protected string m_SpriteFontLocation;
        protected string m_ExtraText;

        public Vector2 Scale { get; set; }
        
        public string Text { get; set; }
        
        public Vector2 Position { get; set; }
        
        public float Rotation { get; set; }
        
        public virtual Color Tint { get; set; }
        
        public string ExtraText
        {
            get
            {
                return m_ExtraText == string.Empty ? m_ExtraText : " : " + m_ExtraText;
            }

            set
            {
                m_ExtraText = value;
            }
        }

        public Vector2 Origin { get; set; }
        
        public Vector2 TextProportion
        {
            get
            {
                return m_SpriteFont.MeasureString(Text + ExtraText);
            }
        }

        public TextComponent(Game i_Game, string i_Text, string i_SpriteFontLocation)
            : base(i_Game)
        {
            Origin = Vector2.Zero;
            m_ExtraText = string.Empty;
            Rotation = 0f;
            Text = i_Text;
            m_SpriteFontLocation = i_SpriteFontLocation;
            Scale = Vector2.One;
            m_SpriteFont = this.Game.Content.Load<SpriteFont>(m_SpriteFontLocation);
        }

        protected override void LoadContent()
        {
            Tint = Color.White;
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch =
                this.Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.DrawString(m_SpriteFont, Text + ExtraText, Position, Tint, Rotation, Origin, Scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public void AlignToCenter()
        {
            Origin = TextProportion / 2;
        }

        protected override void InitBounds()
        {
        }
    }
}
