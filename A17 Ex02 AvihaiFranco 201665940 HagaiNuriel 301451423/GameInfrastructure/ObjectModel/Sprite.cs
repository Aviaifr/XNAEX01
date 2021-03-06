﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Animators;

namespace GameInfrastructure.ObjectModel
{
    public abstract class Sprite : DynamicDrawableComponent
    {
        protected const string k_DeathAnimation = "DeathAnimation";
        protected const string k_HitAnimation = "HitAnimation";
        protected readonly string r_PlayerId = string.Empty;
        protected Vector2 m_Position;
        protected Texture2D m_Texture;
        protected Color m_TintColor;
        protected Game m_Game;
        protected Vector2 m_Velocity;
        public static Random s_RandomGen = new Random();
        protected int m_Width;
        protected int m_Height;
        protected BlendState m_BlendState = BlendState.NonPremultiplied;
        protected bool m_isCollidable = true;
        protected Color[] m_TextureColorData;
        protected Dictionary<string, SoundEffect> m_Sounds = new Dictionary<string, SoundEffect>();

        public Sprite(Game i_Game, string i_TextureString) : base(i_TextureString, i_Game)
        {
        }

        public Sprite(Game i_Game, string i_TextureString, string i_PlayerId)
            : base(i_TextureString, i_Game)
        {
            r_PlayerId = i_PlayerId;
            m_Game = i_Game;
        }

        public Sprite(Game i_Game) : base(i_Game)
        {
        }

        public Sprite(Game i_Game, string i_TesxtureString, int i_CallsOrder) : base(i_TesxtureString, i_Game, i_CallsOrder)
        {
        }

        public SoundEffect GetSound(string i_SoundName)
        {
            SoundEffect returnedSound = null;
            if (m_Sounds.ContainsKey(i_SoundName))
            {
                returnedSound = m_Sounds[i_SoundName];
            }

            return returnedSound;
        }

        public Color Tint
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public bool isCollidable
        {
            get { return m_isCollidable; }
            set { m_isCollidable = value; }
        }

        public virtual bool IsCollidedWith(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            if (source != null)
            {
                if (CanCollideWith(i_Source) && (source.Bounds.Intersects(this.Bounds) || source.Bounds.Contains(this.Bounds)))
                {
                    collided = IsPixelBasedCollision(i_Source);
                }
            }

            return collided;
        }

        protected CompositeAnimator m_Animations;

        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set
            {
                m_WidthBeforeScale = value / m_Scales.X;
                OnPositionChanged();
            }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set
            {
                m_HeightBeforeScale = value / m_Scales.Y;
                OnPositionChanged();
            }
        }

        protected float m_WidthBeforeScale;
        
        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        protected float m_HeightBeforeScale;

        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }

        public Vector2 m_PositionOrigin;

        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public Vector2 m_RotationOrigin = Vector2.Zero;

        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        private Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.Width,
                    (int)this.Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        protected Rectangle m_SourceRectangle = Rectangle.Empty;

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(m_SourceRectangle.Width / 2), (float)(m_SourceRectangle.Height / 2)); }
        }

        protected float m_Rotation = 0;

        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        protected Vector2 m_Scales = Vector2.One;

        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    OnPositionChanged();
                }
            }
        }

        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        protected float m_LayerDepth;

        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;

        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        private float m_AngularVelocity = 0;

        public float AngularVelocity
        {
            get { return m_AngularVelocity; }
            set { m_AngularVelocity = value; }
        }

        public Sprite(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }

        public Sprite(string i_AssetName, Game i_Game, int i_CallsOrder)
            : base(i_AssetName, i_Game, i_CallsOrder)
        {
        }

        public Sprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game, int.MaxValue)
        {
        }

        protected override void InitBounds()
        {
            if (m_SourceRectangle == Rectangle.Empty)
            {
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
            }

            InitSourceRectangle();
            InitOrigins();
            m_WidthBeforeScale = SourceRectangle.Width;
            m_HeightBeforeScale = SourceRectangle.Height;
        }

        protected virtual void InitOrigins()
        {
        }

        protected virtual void InitSourceRectangle()
        {
            if (m_SourceRectangle == Rectangle.Empty)
            {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }
        }

        protected virtual void setupAnimations()
        {
        }

        protected SpriteBatch m_SpriteBatch;

        public SpriteBatch SpriteBatch
        {
            set
            {
                m_SpriteBatch = value;
            }
        }

        public override void Initialize()
        {
            m_Animations = new CompositeAnimator(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
            m_TextureColorData = new Color[m_Texture.Width * m_Texture.Height];
            m_Texture.GetData(m_TextureColorData);
            if (m_SpriteBatch == null)
            {
                m_SpriteBatch =
                    Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                }
            }

            setupAnimations();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.Animations.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin(SpriteSortMode.Deferred, m_BlendState);
            m_SpriteBatch.Draw(
                m_Texture,
                this.PositionForDraw,
                this.SourceRectangle,
                this.Tint,
                this.Rotation,
                this.RotationOrigin,
                this.Scales,
                SpriteEffects.None,
                this.LayerDepth);
            
            m_SpriteBatch.End();
            
            base.Draw(gameTime);
        }

        public virtual bool ActivateAnimation(string i_AnimationName)
        {
            bool AnimationFound = false;

            if(m_Animations != null 
                && m_Animations.Contains(i_AnimationName) 
                && isCollidable)
            {
                AnimationFound = true;
                this.isCollidable = false;
                m_Animations.Restart(i_AnimationName);
            }

            return AnimationFound;
        }

        public Sprite ShallowClone()
        {
            return this.MemberwiseClone() as Sprite;
        }

        public virtual bool CanCollideWith(ICollidable i_Source)
        {
            return true;
        }
        
        public virtual bool IsPixelBasedCollision(ICollidable i_Source)
        {
            bool notFound = true;
            ICollidable2D source = i_Source as ICollidable2D;
            for (int i = 0; i < m_TextureColorData.Length && notFound; i++)
            {
                if (m_TextureColorData[i].A != 0)
                {
                    Point CollidablePointOnScreen = new Point((int)m_Position.X + (i % m_Texture.Width), (int)m_Position.Y + (i / m_Texture.Width));
                    notFound = !source.IsPointInScreenIsColidablePixel(CollidablePointOnScreen);
                }
            }

            return !notFound;
        }

        public virtual bool IsPointInScreenIsColidablePixel(Point i_PointOnScreen)
        {
            bool result = false;
            if (this.Bounds.Contains(i_PointOnScreen))
            {
                int xDifference = i_PointOnScreen.X - (int)m_Position.X;
                int yDifference = i_PointOnScreen.Y - (int)m_Position.Y;
                result = m_TextureColorData[(yDifference * m_Texture.Width) + xDifference].A != 0;
            }

            return result;
        }
    }
}
