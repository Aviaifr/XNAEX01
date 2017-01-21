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
    public abstract class DynamicDrawableComponent : DrawableGameComponent
    {
        public event EventHandler<EventArgs> Disposed;

        public event EventHandler<EventArgs> PositionChanged;

        public event EventHandler<EventArgs> SizeChanged;

        public event EventHandler<EventArgs> Hit;

        public event EventHandler<EventArgs> Destroyed;

        protected string m_AssetName;

        public bool IsVisible { get; set; }

        public virtual Vector2 Position { get; set; }

        public virtual Vector2 Scales { get; set; }

        public virtual float Opacity { get; set; }

        public virtual float Rotation { get; set; }

        public virtual Rectangle SourceRectangle { get; set; }

        public DynamicDrawableComponent(
            string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_Game)
        {
            this.AssetName = i_AssetName;
            this.UpdateOrder = i_UpdateOrder;
            this.DrawOrder = i_DrawOrder;
        }

        public DynamicDrawableComponent(Game i_Game)
            : base(i_Game)
        {   
        }

        public DynamicDrawableComponent(
            string i_AssetName,
            Game i_Game,
            int i_CallsOrder)
            : this(i_AssetName, i_Game, i_CallsOrder, i_CallsOrder)
        { 
        }

        public DynamicDrawableComponent(
            string i_AssetName, Game i_Game)
            : base(i_Game)
        {
            this.AssetName = i_AssetName;
        }

        protected virtual void OnDisposed(object sender, EventArgs args)
        {
            if (Disposed != null)
            {
                Disposed.Invoke(sender, args);
            }
        }

        public virtual DynamicDrawableComponent ShallowClone()
        {
            return this.MemberwiseClone() as DynamicDrawableComponent;
        }

        protected virtual void onDestroyed()
        {
            if(Destroyed != null)
            {
                Destroyed(this, EventArgs.Empty);
            }
        }

        protected virtual void onHit()
        {
            if (Hit != null)
            {
                Hit(this, EventArgs.Empty);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            OnDisposed(this, EventArgs.Empty);
        }

        protected ContentManager ContentManager
        {
            get { return this.Game.Content; }
        }

        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, EventArgs.Empty);
            }
        }

        protected virtual void OnSizeChanged()
        {
            if (SizeChanged != null)
            {
                SizeChanged(this, EventArgs.Empty);
            }
        }

        public string AssetName
        {
            get { return m_AssetName; }
            set { m_AssetName = value; }
        }

        public override void Initialize()
        {
            this.IsVisible = true;
            base.Initialize();

            if (this is ICollidable)
            {
                ICollisionsManager collisionMgr =
                    this.Game.Services.GetService(typeof(ICollisionsManager))
                        as ICollisionsManager;

                if (collisionMgr != null)
                {
                    collisionMgr.AddObjectToMonitor(this as ICollidable);
                }
            }

            InitBounds();
        }

        protected abstract void InitBounds();

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
