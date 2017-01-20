using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class Background : DynamicDrawableComponent
    {
        private Vector2 m_Position;
        public Color Tint { get; set; }
        private Texture2D m_Texture;

        public Background(Game i_Game, string i_TextureString) :
            base(i_TextureString, i_Game, int.MinValue)
        {
            m_Position = Vector2.Zero;
            Tint = Color.Gray;
            Initialize();
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(m_Texture, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Tint);
            spriteBatch.End();
        }

        public override void Update(GameTime i_GameTime)
        {
        }

        protected override void InitBounds()
        {
        }
    }
}
