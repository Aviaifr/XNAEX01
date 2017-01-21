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
    public class Enemy : Sprite, IShootingObject, ICollidable2D
    {
        public int Value { get; set; }

        private static readonly int sr_MaxShots = 1;

        public event EventHandler<EventArgs> Shoot;
        public event EventHandler<EventArgs> Killed;


        private int m_Shots;
        protected float m_timeSinceMoved;
        protected float m_TimeBetweenJumps;
        protected float m_fireChance = 1;
        protected float m_Direction = 1;
        protected int m_NumOfJumps;

        public bool WasHit { get; set; }

        public Enemy(Game i_Game, string i_TextureLocation, int i_Value)
            : base(i_Game, i_TextureLocation)
        {
            Value = i_Value;
            WasHit = false;
        }

        public float FireChance
        {
            get { return m_fireChance; }
            set { m_fireChance = value; }
        }

        public Sprite KilledBy
        {
            get; set;
        } 

        public float Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        public float TimeBetweenJumps
        {
            get { return m_TimeBetweenJumps; }
        }

        public float TimeSinceMoved
        {
            get { return m_timeSinceMoved; }
            set { m_timeSinceMoved = value; }
        }

        public int NumOfJumps
        {
            get { return m_NumOfJumps; }
            set { m_NumOfJumps = value; }
        }

        public override void Initialize()
        {
            m_TimeBetweenJumps = 0.5f;
            base.Initialize();
            initSounds();
            m_timeSinceMoved = 0;
            m_Velocity.X = m_Texture.Width / 2;
            m_TimeBetweenJumps = 0.5f;
            this.RotationOrigin = new Vector2(this.Width / 2, this.Height / 2);
        }
        
        protected override void setupAnimations()
        {
            int cellStartingIndex = 0;
            if (this.SourceRectangle.X != 0)
            {
                cellStartingIndex++;
            }

            SizeAnimator sizeAnimator = 
                new SizeAnimator(TimeSpan.FromSeconds(1.6f), e_SizeType.Shrink);
            RotateAnimator rotateAnimator = 
                new RotateAnimator(TimeSpan.FromSeconds(1.6f), 6);
            CellAnimator cellAnimator = new CellAnimator(TimeSpan.FromSeconds(m_TimeBetweenJumps), 2, TimeSpan.Zero, cellStartingIndex);
            CompositeAnimator compositeAnimator = new CompositeAnimator(
                ObjectValues.DeathAnimation,
                TimeSpan.FromSeconds(1.6f),
                this,
                sizeAnimator,
                rotateAnimator);
            compositeAnimator.Finished += DeathAnimator_Finished;
            m_Animations.Add(compositeAnimator);
            m_Animations.Add(cellAnimator);
            compositeAnimator.Enabled = false;
            cellAnimator.Enabled = true;
            m_Animations.Enabled = true;
        }

        private void DeathAnimator_Finished(object sender, EventArgs e)
        {
            this.WasHit = true;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_Position += m_Velocity * m_NumOfJumps;
            base.Update(i_GameTime);
            tryToShoot();
            OnPositionChanged();
        }

        protected bool hitWall()
        {
            return (m_Position.X + m_Texture.Width >= Game.GraphicsDevice.Viewport.Width && m_Velocity.X > 0)
                || (m_Position.X <= 0 && m_Velocity.X < 0);
        }

        protected virtual void tryToShoot()
        {
            if (m_Shots < sr_MaxShots)
            {
                int randNumToFire = s_RandomGen.Next(0, 100);
                if (randNumToFire < m_fireChance && m_Shots < sr_MaxShots)
                {
                    m_Shots++;
                    OnShoot();
                }
            }
        }

        private void OnShoot()
        {
            if (this.Shoot != null)
            {
                Shoot.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnKilled()
        {
            if (this.Killed != null)
            {
                Killed.Invoke(this, EventArgs.Empty);
            }
        }

        public Vector2 GetShotStartingPosition()
        {
            float x, y;
            x = m_Position.X + (m_Texture.Width / 2);
            y = m_Position.Y + m_Texture.Height;
            return new Vector2(x, y);
        }

        public void ChangeDirection()
        {
            m_Velocity.X *= -1;
        }

        public void OnMyBulletDisappear(object i_SpaceBullet, EventArgs i_EventArgs)
        {
            m_Shots--;
        }

        public virtual void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is SpaceBullet)
            {
                if ((i_Collidable as SpaceBullet).Velocity.Y < 0)
                {
                    this.KilledBy = (i_Collidable as SpaceBullet).Owner;
                    this.OnKilled();
                    this.Dispose();


                }
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
            else if (!(i_Source is Enemy))
            {
                canCollide = true;
            }

            return canCollide;
        }

        internal void UpdateCellSpeed(TimeSpan i_NewCellTime)
        {
            (m_Animations["Cell"] as CellAnimator).CellTime = i_NewCellTime;
        }

        protected virtual void initSounds()
        {
            m_Sounds.Add("shoot", Game.Content.Load<SoundEffect>(@"C:/Temp/XNA_Assets/Ex03/Sounds/EnemyGunShot"));
            m_Sounds.Add("hit", Game.Content.Load<SoundEffect>(@"C:/Temp/XNA_Assets/Ex03/Sounds/EnemyKill"));
        }
    }
}
