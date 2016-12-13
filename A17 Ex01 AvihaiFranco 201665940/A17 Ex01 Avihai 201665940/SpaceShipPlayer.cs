using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class SpaceShipPlayer : Player, I2DPlayer
    {
        private readonly int r_MaxLives = 3;

        public SpaceShipPlayer(DynamicDrawableComponent i_GameComponent) : base(i_GameComponent)
        {
            m_Lives = r_MaxLives;
        }

        public override int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
                m_Score = MathHelper.Clamp(m_Score, 0, int.MaxValue);
            }
        }

        public override int Lives
        {
            get { return m_Lives; }
            set
            {
                m_Lives = value;
            }
        }

        public Vector2 GameComponentPosition
        {
            get
            {
                return (m_GameComponent as Sprite).Position;
            }

            set
            {
                (m_GameComponent as Sprite).Position = value;
            }
        }

        public Rectangle GameComponentBounds
        {
            get
            {
                return (m_GameComponent as Sprite).Bounds;
            }
        }

        protected override void registerToComponentEvents()
        {
            base.registerToComponentEvents();
        }

        protected override void unregisterFromComponentEvents()
        {
            base.unregisterFromComponentEvents();
        }

        protected override void component_Hit(object i_hit, EventArgs i_EventArgs)
        {
            Score -= 1200;
            Lives -= 1;
            if(Lives == 0)
            {
                onPlayerDead();
            }
            else
            {
                onPlayerHit();
            }
        }

        protected override void component_Destroyed(object i_Destroyed, EventArgs i_EventArgs)
        {
            onPlayerDead();
        }
    }
}
