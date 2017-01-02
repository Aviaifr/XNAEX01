using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Animators;
using GameInfrastructure.ObjectModel.Animators.ConcreteAnimators;

namespace Space_Invaders
{
    public class MothershipEnemy : Enemy
    {
        public event EventHandler<EventArgs> MothershipKilled;

        private static readonly Vector2 sr_MothershipSpeed = new Vector2(105f, 0f);
        private static readonly int sr_AppearChance = 1;

        public MothershipEnemy(Game i_Game, string i_TextureLocation, int i_Value)
            : base(i_Game, i_TextureLocation, i_Value)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Velocity = sr_MothershipSpeed;
            this.IsVisible = false;
            m_TintColor = Color.Red;
            m_Position.X -= this.Width;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.IsVisible)
            {
                m_Position += m_Velocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                if (this.Position.X >= Game.GraphicsDevice.Viewport.Width)
                {
                    resetMothership();
                }
            }
            else
            {
                tryToAppear();
            }

            m_Animations.Update(i_GameTime);
            
        }

        private void resetMothership()
        {
            this.IsVisible = false;
            this.Position *= new Vector2(0, 1);
            m_Position.X -= this.Width;
            m_Velocity = sr_MothershipSpeed;
            isCollidable = true;
        }

        protected override void setupAnimations()
        {
            SizeAnimator sizeAnimator = 
                new SizeAnimator(TimeSpan.FromSeconds(2.4f), e_SizeType.Shrink);
            BlinkAnimator blinkAnimator = 
                new BlinkAnimator(TimeSpan.FromSeconds(0.2f),TimeSpan.FromSeconds(2.4f));
            FadeAnimator fadeAnimator = 
                new FadeAnimator(TimeSpan.FromSeconds(2.4f));

            CompositeAnimator compositeAnimator = new CompositeAnimator
                (ObjectValues.sr_DeathAnimation, TimeSpan.FromSeconds(2.4), 
                this, sizeAnimator, blinkAnimator, fadeAnimator);

            compositeAnimator.Finished += DeathAnimator_Finished;
            m_Animations.Add(compositeAnimator);
            compositeAnimator.Enabled = false;
            m_Animations.Enabled = true;
        }

        private void DeathAnimator_Finished(object sender, EventArgs e)
        {
            //this.Animations.Reset(ObjectValues.sr_DeathAnimation);
            //this.Animations.Disable(ObjectValues.sr_DeathAnimation);
            resetMothership();
        }

        private void tryToAppear()
        {
            int randomToAppear = s_RandomGen.Next(0, 5);
            if (sr_AppearChance >= randomToAppear)
            {
                this.IsVisible = true;
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (MothershipKilled != null)
            {
                MothershipKilled.Invoke(this, EventArgs.Empty);
            }
        }

        public override bool CanCollideWith(ICollidable i_Source)
        {
            bool canCollide = false;
            if (i_Source is SpaceBullet)
            {
                if ((i_Source as SpaceBullet).Velocity.Y < 0)
                {
                    canCollide = true;
                }
            }
            return canCollide;
        }
    }
}
