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
        protected virtual void OnDisposed(object sender, EventArgs args)
        {
            if (Disposed != null)
            {
                Disposed.Invoke(sender, args);
            }
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            OnDisposed(this, EventArgs.Empty);
        }

        protected string m_AssetName;

        protected ContentManager ContentManager
        {
            get { return this.Game.Content; }
        }

        public event EventHandler<EventArgs> PositionChanged;
        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> SizeChanged;
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

        public DynamicDrawableComponent(
            string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_Game)
        {
            this.AssetName = i_AssetName;
            this.UpdateOrder = i_UpdateOrder;
            this.DrawOrder = i_DrawOrder;

            //this.Game.Components.Add(this);
        }

        public DynamicDrawableComponent(
            string i_AssetName,
            Game i_Game,
            int i_CallsOrder)
            : this(i_AssetName, i_Game, i_CallsOrder, i_CallsOrder)
        { }

        public DynamicDrawableComponent(
            string i_AssetName, Game i_Game)
            : base(i_Game)
        {
            this.AssetName = i_AssetName;

            //this.Game.Components.Add(this);
        }

        public DynamicDrawableComponent(Game i_Game) : base(i_Game)
        {
            //this.Game.Components.Add(this);
        }

        public override void Initialize()
        {
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

            InitBounds();   // a call to an abstract method;
        }

#if DEBUG
        protected bool m_ShowBoundingBox = true;
#else
        protected bool m_ShowBoundingBox = false;
#endif

        public bool ShowBoundingBox
        {
            get { return m_ShowBoundingBox; }
            set { m_ShowBoundingBox = value; }
        }

        protected abstract void InitBounds();

        public override void Draw(GameTime gameTime)
        {
            //DrawBoundingBox();
            base.Draw(gameTime);
        }

        //protected abstract void DrawBoundingBox(); TODO: check when we really need this
    }
}
