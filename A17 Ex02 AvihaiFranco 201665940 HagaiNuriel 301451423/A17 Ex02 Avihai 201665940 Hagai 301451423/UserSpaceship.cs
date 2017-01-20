using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
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

        private readonly Vector2 m_BeginningPosition;

        private bool m_IsFirstUpdate = true;
        private IPlayersManager m_PlayersManager;

        private int m_Shots;

        public UserSpaceship(Game i_Game, string i_TextureString)
            : base(i_Game, i_TextureString)
        {
            m_Shots = 0;
            this.Tint = Color.White;
            m_BlendState = BlendState.NonPremultiplied;
        }

        public Vector2 BeginningPosition
        {
            get { return m_BeginningPosition; }
        }

        public UserSpaceship(Game i_Game, string i_TextureString, string i_PlayerId, Vector2 i_BeginningPosition)
            : base(i_Game, i_TextureString, i_PlayerId)
        {
            m_Shots = 0;
            this.Tint = Color.White;
            m_BlendState = BlendState.NonPremultiplied;
            m_BeginningPosition = i_BeginningPosition;
        }

        public override void Initialize()
        {
            base.Initialize();
            initSounds();
            this.RotationOrigin = new Vector2(this.Width / 2, this.Height / 2);
        }

        private void initSounds()
        {
            m_Sounds.Add("shoot", Game.Content.Load<SoundEffect>(@"C:/Temp/XNA_Assets/Ex03/Sounds/SSGunShot"));
            m_Sounds.Add("hit", Game.Content.Load<SoundEffect>(@"C:/Temp/XNA_Assets/Ex03/Sounds/LifeDie"));
        }

        protected override void setupAnimations()
        {
            BlinkAnimator blinkAnimator = 
                new BlinkAnimator(
                    ObjectValues.HitAnimation,
                    TimeSpan.FromSeconds(0.07),
                    TimeSpan.FromSeconds(2.4));

            blinkAnimator.Finished += HitAnimator_Finished;
            m_Animations.Add(blinkAnimator);
            m_Animations.Disable(ObjectValues.HitAnimation);

            FadeAnimator fadeAnimator = 
                new FadeAnimator(TimeSpan.FromSeconds(2.4f));
            RotateAnimator rotateAnimator = 
                new RotateAnimator(TimeSpan.FromSeconds(2.4f), 4);
            CompositeAnimator compositeAnimator = new CompositeAnimator(
                ObjectValues.DeathAnimation,
                TimeSpan.FromSeconds(2.4f),
                this,
                fadeAnimator,
                rotateAnimator);
            compositeAnimator.Finished += DeathCompositeAnimator_Finished;
            compositeAnimator.ResetAfterFinish = false;
            m_Animations.Add(compositeAnimator);
            m_Animations.Disable(ObjectValues.DeathAnimation);
            
            m_Animations.Enabled = true;
        }

        private void DeathCompositeAnimator_Finished(object sender, EventArgs e)
        {
            Game.Components.Remove(this);
        }

        private void HitAnimator_Finished(object sender, EventArgs e)
        {
            isCollidable = true;
        }

        protected override void LoadContent()
        {
            m_PlayersManager = m_Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
            base.LoadContent();
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
            if (m_IsFirstUpdate)
            {
                m_IsFirstUpdate = false;
            }
            else
            {
                if(r_PlayerId != string.Empty)
                {
                    if (m_PlayersManager.PlyersInfo[r_PlayerId].IsMouseControlled)
                    {
                        m_Position.X += i_InputManager.MousePositionDelta.X;
                    }
                }
            }
        }

        private void updateSpeedByInput(IInputManager i_InputManager)
        {
            m_Velocity = Vector2.Zero;
            if(r_PlayerId != string.Empty)
            {
                if(m_PlayersManager.DidPress(r_PlayerId, eActions.Left))
                {
                    m_Velocity.X = -sr_SpaceshipSpeed;
                }
                else if (m_PlayersManager.DidPress(r_PlayerId, eActions.Right))
                {
                    m_Velocity.X = sr_SpaceshipSpeed;
                }
            }
        }

        private void checkInputForShot(IInputManager i_InputManager)
        {
            if(m_PlayersManager.DidPress(r_PlayerId, eActions.Shoot) && !isDying())
            {
                OnShoot();
            }
        }

        private bool isDying()
        {
            return m_Animations[ObjectValues.DeathAnimation].Enabled
                && !m_Animations[ObjectValues.DeathAnimation].IsFinished;
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
            if (i_Source is MothershipEnemy || i_Source is UserSpaceship || !isCollidable)
            {
                canCollide = false;
            }
            else if (i_Source is SpaceBullet || (i_Source is Enemy))
            {
                canCollide = true;
                if (i_Source is SpaceBullet)
                {
                    canCollide = (i_Source as SpaceBullet).Velocity.Y > 0;
                }
            }

            return canCollide;
        }
    }   
}
