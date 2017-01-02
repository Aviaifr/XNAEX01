using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Animators.ConcreteAnimators;
using GameInfrastructure.ObjectModel.Animators;

namespace Space_Invaders
{
    public class UserSpaceship : Sprite, IShootingObject, ICollidable2D
    {
        private static readonly int sr_SpaceshipSpeed = 160;
        private static readonly int sr_MaxShots = 2;
        public event EventHandler<EventArgs> Shoot;

        private int m_Shots;

        public UserSpaceship(Game i_Game, string i_TextureString)
            : base(i_Game, i_TextureString)
        {
            m_Shots = 0;
            this.Tint = Color.White;
            m_BlendState = BlendState.NonPremultiplied;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.RotationOrigin = new Vector2(this.Width / 2, this.Height / 2);
        }

        protected override void setupAnimations()
        {
            BlinkAnimator blinkAnimator = 
                new BlinkAnimator(ObjectValues.sr_HitAnimation,TimeSpan.FromSeconds(0.14), TimeSpan.FromSeconds(2.4));
            //blinkAnimator.Finished += BlinkAnimator_Finished;
            m_Animations.Add(blinkAnimator);
            m_Animations.Disable(ObjectValues.sr_HitAnimation);

            FadeAnimator fadeAnimator = 
                new FadeAnimator(TimeSpan.FromSeconds(2.4f));
            RotateAnimator rotateAnimator = 
                new RotateAnimator(TimeSpan.FromSeconds(2.4f), 4);
            CompositeAnimator compositeAnimator = 
                new CompositeAnimator(ObjectValues.sr_DeathAnimation, TimeSpan.FromSeconds(2.4f), this, fadeAnimator, rotateAnimator);
            compositeAnimator.Finished += DeathCompositeAnimator_Finished;
            compositeAnimator.ResetAfterFinish = false;
            m_Animations.Add(compositeAnimator);
            m_Animations.Disable(ObjectValues.sr_DeathAnimation);
            
            m_Animations.Enabled = true;
        }

        private void DeathCompositeAnimator_Finished(object sender, EventArgs e)
        {
            Game.Components.Remove(this);
        }

        private void BlinkAnimator_Finished(object sender, EventArgs e)
        {
            m_Animations.Reset(ObjectValues.sr_BlinkingAnimator);
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            IInputManager inputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            updateSpeedByInput(inputManager);
            m_Position.X += m_Velocity.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            updatePositionByMouse(inputManager);
            m_Position.X = MathHelper.Clamp(m_Position.X, 0, Game.GraphicsDevice.Viewport.Width - m_Texture.Width);
            checkInputForShot(inputManager);
            OnPositionChanged();
        }

        private void updatePositionByMouse(IInputManager i_InputManager)
        {
            m_Position.X += i_InputManager.MousePositionDelta.X;
        }

        private void updateSpeedByInput(IInputManager i_InputManager)
        {
            m_Velocity = Vector2.Zero;
            if (i_InputManager.KeyHeld(Keys.Left))
            {
                m_Velocity.X = -sr_SpaceshipSpeed;
            }
            else if (i_InputManager.KeyHeld(Keys.Right))
            {
                m_Velocity.X = sr_SpaceshipSpeed;
            }
        }

        private void checkInputForShot(IInputManager i_InputManager)
        {
            if ((i_InputManager.KeyPressed(Keys.Enter) || i_InputManager.ButtonPressed(eInputButtons.Left)) && m_isCollidable)
            {
                OnShoot();
            }
        }

        private void OnShoot()
        {
            if (m_Shots < sr_MaxShots)
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

        public void Collided(ICollidable i_Collidable)
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
        }

        public override bool CanCollideWith(ICollidable i_Source)
        {
            bool canCollide = true;
            if (i_Source is MothershipEnemy)
            {
                canCollide = false;
            }
            else if (i_Source is SpaceBullet)
            {
                if ((i_Source as SpaceBullet).Velocity.Y < 0)
                {
                    canCollide = false;
                }
            }
            return canCollide;
        }
    }   
}
