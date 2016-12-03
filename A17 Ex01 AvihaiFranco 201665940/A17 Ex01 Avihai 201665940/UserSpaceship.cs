using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public class UserSpaceship : SpaceObject
    {
        public static int s_SpaceshipSpeed = 135;

        private MouseState? m_PrevioussMouseState;

        public UserSpaceship(Game i_Game, string i_TextureString)
            : base(i_Game, i_TextureString)
        {
            m_PrevioussMouseState = null;
        }

        public override void Update(GameTime i_GameTime)
        {
            UpdateSpeedByKeyboard();
            m_Position.X += m_Speed.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            m_Position.X += getMousePositionDelta().X;
            m_Position.X = MathHelper.Clamp(m_Position.X, 0, m_Game.GraphicsDevice.Viewport.Width - m_Texture.Width);
        }

        private void UpdateSpeedByKeyboard()
        {
            KeyboardState currKeyboardState = Keyboard.GetState();
            if (currKeyboardState.IsKeyDown(Keys.Left))
            {
                m_Speed = new Vector2(-s_SpaceshipSpeed, 0);
            }
            else if (currKeyboardState.IsKeyDown(Keys.Right))
            {
                m_Speed = new Vector2(s_SpaceshipSpeed, 0);
            }
            else if (currKeyboardState.IsKeyUp(Keys.Right) || currKeyboardState.IsKeyUp(Keys.Left))
            {
                m_Speed = Vector2.Zero;
            }
        }

        private Vector2 getMousePositionDelta()
        {
            Vector2 retVal = Vector2.Zero;
            MouseState currState = Mouse.GetState();
            if (m_PrevioussMouseState != null)
            {
                retVal.X = currState.X - m_PrevioussMouseState.Value.X;
                retVal.Y = currState.Y - m_PrevioussMouseState.Value.Y;
            }

            m_PrevioussMouseState = currState;
            return retVal;
        }
    }
}
