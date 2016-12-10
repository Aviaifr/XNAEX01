using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace A17_Ex01_Avihai_201665940
{
    public class Background : Sprite
    {
        //gives its father's constructor a minimum order value so it will always be drawn first
        public Background(Game i_Game, string i_TextureString) : 
            base(i_Game, i_TextureString, int.MinValue)
        {
            m_TextureString = i_TextureString;
            m_Position = Vector2.Zero;
            m_Tint = Color.Gray;
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
            spriteBatch.Draw(m_Texture, new Rectangle
                (0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), m_Tint);
            spriteBatch.End();
            //base.Draw(gameTime);
        }

        public override void Update(GameTime i_GameTime)
        {
        }
    }
}
