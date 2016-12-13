using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using GameInfrastructure.ServiceInterfaces;

namespace GameInfrastructure.ObjectModel
{
    public abstract class Player : IPlayer
    {
        protected int m_Score;
        protected int m_Lives;
        protected DynamicDrawableComponent m_GameComponent;
        public event EventHandler<EventArgs> PlayerDead;
        public event EventHandler<EventArgs> PlayerHit;

        public Player(DynamicDrawableComponent i_GameComponent)
        {
            m_GameComponent = i_GameComponent;
            registerToComponentEvents();
        }

        public virtual int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public virtual int Lives
        {
            get { return m_Lives; }
            set { m_Lives = value; }
        }

        public virtual DynamicDrawableComponent GameComponent
        {
            get { return m_GameComponent; }
            set
            {
                if(m_GameComponent != null)
                {
                    unregisterFromComponentEvents();
                }
                m_GameComponent = value;
                registerToComponentEvents();
            }
        }


        protected virtual void onPlayerDead()
        {
            if(PlayerDead != null)
            {
                PlayerDead(this, EventArgs.Empty);
            }
        }

        protected virtual void onPlayerHit()
        {
            if(PlayerHit != null)
            {
                PlayerHit(this, EventArgs.Empty);
            }
        }

        protected virtual void registerToComponentEvents()
        {
            m_GameComponent.Hit += component_Hit;
            m_GameComponent.Destroyed += component_Destroyed;
        }
        protected virtual void unregisterFromComponentEvents()
        {
            m_GameComponent.Hit -= component_Hit;
            m_GameComponent.Destroyed -= component_Destroyed;
        }

        protected abstract void component_Hit(object i_HitComponent, EventArgs i_EventArgs);
        protected abstract void component_Destroyed(object i_DestroyedComponent, EventArgs i_EventArgs);

    }
}
