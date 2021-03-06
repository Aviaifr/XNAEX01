﻿using System;
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
using GameInfrastructure.ObjectModel.Animators;

namespace Space_Invaders
{
    public class SpaceShipPlayer : Player, I2DPlayer
    {
        private readonly int r_MaxLives = 3;

        public SoulsBatch SoulBatch { get; set; }

        private ScoreBoard m_ScoreBoard;

        public SpaceShipPlayer(DynamicDrawableComponent i_GameComponent, string i_PlayerId) : base(i_GameComponent, i_PlayerId)
        {
            m_Lives = r_MaxLives;
        }

        public ScoreBoard ScoreBoard
        {
            get { return m_ScoreBoard; }
            set { m_ScoreBoard = value; }
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
                this.ScoreBoard.ScoreValue = m_Score;
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

        public CompositeAnimator GameComponenetAnimations
        {
            get
            {
                return (m_GameComponent as Sprite).Animations;
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
            SoulBatch.RemoveSoul();
            if (Lives == 0)
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
