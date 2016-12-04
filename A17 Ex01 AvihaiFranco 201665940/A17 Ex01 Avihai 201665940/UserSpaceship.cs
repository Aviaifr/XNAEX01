using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public class UserSpaceship : SpaceObject, IShootingObject
    {
        public static int s_SpaceshipSpeed = 135;

        public event ObjectFireHandler OnUserFire;

        private MouseState? m_PrevioussMouseState;
        private KeyboardState? m_PrevioussKeyboardState;
        private int m_Shots;

        public UserSpaceship(Game i_Game, string i_TextureString)
            : base(i_Game, i_TextureString)
        {
            m_PrevioussMouseState = null;
            m_PrevioussKeyboardState = null;
            m_Shots = 0;
        }

        public override void Update(GameTime i_GameTime)
        {
            handleKeyboard();
            m_Position.X += m_Speed.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            m_Position.X += getMousePositionDelta().X;
            m_Position.X = MathHelper.Clamp(m_Position.X, 0, m_Game.GraphicsDevice.Viewport.Width - m_Texture.Width);
        }

        private void handleKeyboard()
        {
            KeyboardState currKeyboardState = Keyboard.GetState();
            UpdateSpeedByKeyboard(currKeyboardState);
            checkIfUserShot(currKeyboardState);
            m_PrevioussKeyboardState = currKeyboardState;
        }

        private void checkIfUserShot(KeyboardState i_currentState)
        {
            MouseState currentMouseState = Mouse.GetState();
            if (m_PrevioussMouseState != null && m_PrevioussMouseState.Value.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
            {
                Shoot();
            }
            else if (m_PrevioussKeyboardState != null && m_PrevioussKeyboardState.Value.IsKeyDown(Keys.Enter) && i_currentState.IsKeyUp(Keys.Enter))
            {
                Shoot();
            }
        }

        public void Shoot()
        {
            if (m_Shots < 2)
            {
                if (OnUserFire != null)
                {
                    m_Shots++;
                    OnUserFire.Invoke(this);
                }
            }     
        }

        public Vector2 GetShotStartingPosition()
        {
            float x, y;
            x = m_Position.X + (m_Texture.Width / 2);
            y = m_Position.Y;
            return new Vector2(x, y);
        }

        private void UpdateSpeedByKeyboard(KeyboardState i_currentState)
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

        public void OnMybulletDisappear(SpaceBullet i_DisappearedBullet)
        {
            m_Shots--;
        }
    }
}
