using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace A17_Ex01_Avihai_201665940
{
    public class UserSpaceship : Sprite, IShootingObject, ICollidable2D
    {
        private static readonly int sr_SpaceshipSpeed = 135;
        private MouseState? m_PrevMouseState;

        public event EventHandler<EventArgs> Shoot;

        private int m_Shots;

        public UserSpaceship(Game i_Game, string i_TextureString)
            : base(i_Game, i_TextureString)
        {
            m_Shots = 0;
            this.Tint = Color.White;
        }

        public override void Update(GameTime i_GameTime)
        {
            IInputManager inputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            updateSpeedByInput(inputManager);
            m_Position.X += m_Speed.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            updatePositionByMouse(inputManager);
            m_Position.X = MathHelper.Clamp(m_Position.X, 0, Game.GraphicsDevice.Viewport.Width - m_Texture.Width);
            checkInputForShot(inputManager);
            OnPositionChanged();
        }

        private void updatePositionByMouse(IInputManager i_InputManager)
        {
            //inputmanager does not initialize the prevState to be nullable in order to move the shi relative to its starting point
            MouseState currState = Mouse.GetState();
            Vector2 mouseDelta = Vector2.Zero;
            if(m_PrevMouseState != null)
            {
                mouseDelta.X = currState.X - m_PrevMouseState.Value.X;
                mouseDelta.Y = currState.Y - m_PrevMouseState.Value.Y;
            }

            m_Position.X += mouseDelta.X;
            m_PrevMouseState = currState;
        }

        private void updateSpeedByInput(IInputManager i_InputManager)
        {
            m_Speed = Vector2.Zero;
            if (i_InputManager.KeyHeld(Keys.Left))
            {
                m_Speed.X = -sr_SpaceshipSpeed;
            }
            else if (i_InputManager.KeyHeld(Keys.Right))
            {
                m_Speed.X = sr_SpaceshipSpeed;
            }
        }

        private void checkInputForShot(IInputManager i_InputManager) //TODO: added space for convenience
        {
            if (i_InputManager.KeyPressed(Keys.Enter) || i_InputManager.KeyPressed(Keys.Space) || i_InputManager.ButtonPressed(eInputButtons.Left))
            {
                OnShoot();
            }
        }

        private void OnShoot()
        {
            if (m_Shots < 2)
            {
                if (Shoot != null)
                {
                    m_Shots++;
                    Shoot.Invoke(this, EventArgs.Empty);
                }
            }     
        }

        public void OnMyBulletDisappear(object i_SpaceBullet, EventArgs i_EventArgs)
        {
            m_Shots--;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if(i_Collidable is SpaceBullet)
            {
                if((i_Collidable as SpaceBullet).Velocity.Y > 0)
                {
                    onHit();
                }
            }
            else if(i_Collidable is Enemy)
            {
                onDestroyed();
            }
            base.Collided(i_Collidable);
        }

    }

    
}
