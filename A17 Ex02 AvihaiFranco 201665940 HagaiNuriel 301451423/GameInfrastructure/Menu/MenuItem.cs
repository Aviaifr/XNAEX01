using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.Menu;

namespace GameInfrastructure.Menu
{
    public class MenuItem : DynamicDrawableComponent
    {
        public event EventHandler Choose;
        private string m_OptionName;
        private SpriteFont m_SpriteFont;
        private string m_SpriteFontLocation;
        private Color m_ActiveTint;
        private Color m_InActiveTint;
        protected string m_ExtraText;

        public bool isActive { get; set; }
        public string Name { get { return m_OptionName; } }
        public Vector2 Position { get; set; }
        public Color Tint { get { return isActive ? m_ActiveTint : m_InActiveTint; } }
        public string ExtraText { get { return m_ExtraText == string.Empty ? m_ExtraText : " : " + m_ExtraText; } set { m_ExtraText = value; } }
        public MenuItem(Game i_Game, string i_Name, string i_SpriteFontLocation, Color i_ActiveTint, Color i_InActiveTint)
            : base(i_Game)
        {
            m_ExtraText = string.Empty;
            m_ActiveTint = i_ActiveTint;
            m_InActiveTint = i_InActiveTint;
            m_OptionName = i_Name;
            m_SpriteFontLocation = i_SpriteFontLocation;
            isActive = false;
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
            spriteBatch.DrawString(m_SpriteFont, m_OptionName + ExtraText, Position, Tint);
            spriteBatch.End();
        }

        protected void onChooseOption()
        {
            if (Choose != null && isActive)
            {
                Choose(this, EventArgs.Empty);
            }
        }
        protected override void InitBounds()
        {
        }
    }
}
