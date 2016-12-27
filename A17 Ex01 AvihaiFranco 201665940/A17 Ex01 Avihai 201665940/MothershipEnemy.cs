using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

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
            m_Speed = sr_MothershipSpeed;
            this.IsVisible = false;
            m_Tint = Color.Red;
            m_Position.X -= this.Width;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.IsVisible)
            {
                m_Position += m_Speed * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                if (this.Position.X >= Game.GraphicsDevice.Viewport.Width)
                {
                    resetMothership();
                }
            }
            else
            {
                tryToAppear();
            }
        }

        private void resetMothership()
        {
            this.IsVisible = false;
            this.Position *= new Vector2(0, 1);
            m_Position.X -= this.Width;
        }

        private void tryToAppear()
        {
            int randomToAppear = s_RandomGen.Next(0, 700);
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

            resetMothership();
        }
    }
}
