using System;
using System.Collections.Generic;
using GameInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using GameInfrastructure.ServiceInterfaces;

namespace GameInfrastructure.Managers
{
    public class CollisionsManager : GameService, ICollisionsManager
    {
        protected readonly List<ICollidable> m_Collidables = new List<ICollidable>();

        public CollisionsManager(Game i_Game) :
            base(i_Game, int.MaxValue)
        { 
        }

        public void ClearCollidable()
        {
            m_Collidables.Clear();
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ICollisionsManager), this);
        }

        public void AddObjectToMonitor(ICollidable i_Collidable)
        {
            if (!this.m_Collidables.Contains(i_Collidable))
            {
                this.m_Collidables.Add(i_Collidable);
                i_Collidable.PositionChanged += collidable_Changed;
                i_Collidable.SizeChanged += collidable_Changed;
                i_Collidable.VisibleChanged += collidable_Changed;
                i_Collidable.Disposed += collidable_Disposed;
            }
        }

        private void collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;

            if (collidable != null
                &&
                this.m_Collidables.Contains(collidable))
            {
                collidable.PositionChanged -= collidable_Changed;
                collidable.SizeChanged -= collidable_Changed;
                collidable.VisibleChanged -= collidable_Changed;
                collidable.Disposed -= collidable_Disposed;

                m_Collidables.Remove(collidable);
            }
        }

        private void collidable_Changed(object sender, EventArgs e)
        {
            if (sender is ICollidable)
            {
                checkCollision(sender as ICollidable);
            }
        }

        private void checkCollision(ICollidable i_Source)
        {
            if (i_Source.IsVisible)
            {
                List<ICollidable> collidedComponents = new List<ICollidable>();

                foreach (ICollidable target in m_Collidables)
                {
                    if (i_Source != target && target.IsVisible)
                    {
                        if (target.IsCollidedWith(i_Source))
                        {
                            if (i_Source.IsCollidedWith(target))
                            {
                                collidedComponents.Add(target);
                            }
                        }
                    }
                }

                foreach (ICollidable target in collidedComponents)
                {
                    target.Collided(i_Source);
                    i_Source.Collided(target);
                }
            }
        }
    }
}
