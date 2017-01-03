using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class Background : Sprite
    {
        public Background(Game i_Game, string i_TextureString) : 
            base(i_Game, i_TextureString, int.MinValue)
        {
            m_Position = Vector2.Zero;
            m_TintColor = Color.Gray;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(m_Texture, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), m_TintColor);
            spriteBatch.End();
        }

        public override void Update(GameTime i_GameTime)
        {
        }
    }
}
