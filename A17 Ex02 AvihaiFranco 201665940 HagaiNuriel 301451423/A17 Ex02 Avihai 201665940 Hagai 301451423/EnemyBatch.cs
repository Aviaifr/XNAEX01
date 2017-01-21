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
using GameInfrastructure.ObjectModel.Screens;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class EnemyBatch : Sprite
    {
        private readonly int[] r_InitialEnemyValues = new int[] { 240, 170, 140 };
        private readonly int r_MotherShipInitValue = 650;
        private readonly int r_VerticalPadding = 32 * 3;
        private readonly float r_VerticalJumpLength = 32 * 0.5f;
        private readonly string r_EnemiesTextureString = @"Enemies/EnemiesTexture";
        private readonly int r_Enemy1IndexInTexture = 0;
        private readonly int r_Enemy2IndexInTexture = 1;
        private readonly int r_Enemy3IndexInTexture = 2;
        private readonly Color r_Enemy1Tint = Color.LightPink;
        private readonly Color r_Enemy2Tint = Color.LightBlue;
        private readonly Color r_Enemy3Tint = Color.LightYellow;
        private readonly Color r_EnemyBulletTint = Color.Blue;
        private int m_BatchColumns;
        private int m_BatchRows;
        private float m_EnemyFireChance;   
        private int m_Enemy1Value;
        private int m_Enemy2Value;
        private int m_Enemy3Value;
        private int m_MotherShipValue;
        private readonly float r_EnemySize = 32;
        private readonly Color r_EnemyButtletTint = Color.Blue;
        public GameScreen PlayScreen { get; set; }

        public void IncreaseEnemyScores(int i_Value)
        {
            m_Enemy1Value += i_Value;
            m_Enemy2Value += i_Value;
            m_Enemy3Value += i_Value;
            m_MotherShipValue += i_Value;
        }

        public void ResetEnemyValues()
        {
            m_Enemy1Value = r_InitialEnemyValues[0];
            m_Enemy2Value = r_InitialEnemyValues[1];
            m_Enemy3Value = r_InitialEnemyValues[2];
            m_MotherShipValue = r_MotherShipInitValue; 
        }

        public int EnemyCols
        {
            get { return m_BatchColumns; }
            set { m_BatchColumns = value; }
        }

        public float EnemyFireChance
        {
            get { return m_EnemyFireChance; }
            set { m_EnemyFireChance = value; }
        }

        public int EnemyRows
        {
            get { return m_BatchRows; }
            set { m_BatchRows = value; }
        }

        public event EventHandler<EventArgs> EnemyKilled;

        public event EventHandler<EventArgs> NoMoreEnemies;

        public event EventHandler<EventArgs> EnemyReachedBottom;
        private const float k_InitTimeBetweenJumps = 0.5f;
        private List<Enemy> m_Enemies;
        private MothershipEnemy m_MotherShip;
        private int m_maxY = 0;
        private bool m_EnemyHitWall;
        private float m_XMax, m_XMin;
        private bool m_BatchMovingRight = true;
        private float m_TimeSinceMoved = 0f;
        private float m_TimeBetweenJumps = 0.5f;

        public int EnemyCount
        { 
            get
            {
                return this.m_Enemies.Count - m_Enemies.Count<Enemy>(enemy => enemy.WasHit == true);
            }
        }

        public EnemyBatch(Game i_Game) : base(i_Game)
        {
            m_Enemies = new List<Enemy>();
            m_BatchColumns = 9;
            m_BatchRows = 5;
            m_EnemyFireChance = 1;
            m_Enemy1Value = 240;
            m_Enemy2Value = 170;
            m_Enemy3Value = 140;
            m_MotherShipValue = 650;
       }

        protected override void LoadContent()
        {
        }

        public override void Initialize()
        {
            m_MotherShip = new MothershipEnemy(this.Game, ObjectValues.MothershipTextureString, m_MotherShipValue);
            m_MotherShip.Position = new Vector2(0, ObjectValues.EnemyWidth);
            m_MotherShip.MothershipKilled += onComponentDisposed;
            m_MotherShip.Initialize();
            for (int i = 0; i < m_BatchRows; i++)
            {
                for(int j = 0; j < m_BatchColumns; j++)
                {
                    float x = (float)(j * (1.6 * r_EnemySize));
                    float y = (float)(r_VerticalPadding + (i * 1.6 * r_EnemySize));
                    Enemy newEnemy = new Enemy(Game, r_EnemiesTextureString, GetEnemyValueByRow(i));
                    newEnemy.SourceRectangle = new Rectangle(GetEnemyXLocation(i), (GetEnemyIndexInTextureByRow(i) * (int)r_EnemySize) + 1, (int)r_EnemySize, (int)r_EnemySize);
                    newEnemy.Tint = GetEnemyTintByRow(i);
                    newEnemy.Position = new Vector2(x, y);
                    newEnemy.FireChance = this.EnemyFireChance;
                    newEnemy.Shoot += enemy_OnShoot;
                    newEnemy.Killed += onComponentDisposed;
                    newEnemy.Initialize();
                    m_Enemies.Add(newEnemy);
                }
            }

            this.SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        private void enemy_OnShoot(object i_Sender, EventArgs i_EventArgs)
        {
            SpaceBullet newBullet = new SpaceBullet
                (this.Game, ObjectValues.BulletTextureString, r_EnemyBulletTint);
            (Game.Services.GetService(typeof(ISoundEffectsPlayer)) as ISoundEffectsPlayer).PlaySoundEffect((i_Sender as Enemy).GetSound("shoot"));
            newBullet.Initialize();
            newBullet.Owner = i_Sender as Sprite;
            newBullet.Disposed += onComponentDisposed;
            setNewEnemyBulletPosition(i_Sender as Enemy, newBullet);
            newBullet.Disposed += (i_Sender as Enemy).OnMyBulletDisappear;
            PlayScreen.Add(newBullet);
        }

        public void onComponentDisposed(object i_Disposed, EventArgs i_EventArgs)
        {
            if (m_Enemies.Contains(i_Disposed) || i_Disposed is MothershipEnemy)
            {
                if (m_Enemies.Contains(i_Disposed))
                {
                    Enemy enemy = i_Disposed as Enemy;
                    if (enemy.ActivateAnimation(ObjectValues.DeathAnimation))
                    {
                        speedUpEnemies();
                    }
                }

                if (EnemyKilled != null)
                {
                    EnemyKilled(i_Disposed, i_EventArgs);
                }
            }
            (Game.Services.GetService(typeof(IScreensMananger)) as IScreensMananger).ActiveScreen.Remove(i_Disposed as IGameComponent);
        }

        private void setNewEnemyBulletPosition(Enemy i_EnemyShot, SpaceBullet i_newBullet)
        {
            Vector2 newBulletPos = i_EnemyShot.Position;
            newBulletPos.X += i_EnemyShot.Width / 2;
            newBulletPos.X -= i_newBullet.Width / 2;
            newBulletPos.Y += i_EnemyShot.Height;
            i_newBullet.Position = newBulletPos;
        }

        protected override void OnDisposed(object sender, EventArgs args)
        {
            foreach(Enemy enemy in m_Enemies)
            {
                enemy.Dispose();
            }

            base.OnDisposed(sender, args);
        }

        public void Reset()
        {
            foreach (Enemy enemy in m_Enemies)
            {
                enemy.Dispose();
            }

            m_MotherShip.Dispose();
            m_Enemies.Clear();
            m_MotherShip = null;
            m_BatchMovingRight = true;
            m_TimeBetweenJumps = k_InitTimeBetweenJumps;
            Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            m_TimeSinceMoved += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimeSinceMoved >= m_TimeBetweenJumps)
            {
                if (m_EnemyHitWall)
                {
                    m_EnemyHitWall = false;
                    speedUpEnemies();
                    foreach (Enemy enemy in m_Enemies)
                    {
                        Vector2 newPosition = enemy.Position;
                        newPosition.Y += r_VerticalJumpLength;
                        enemy.Position = newPosition;
                        enemy.ChangeDirection();
                    }
                }
                else
                {
                    m_XMax = 0;
                    m_XMin = Game.GraphicsDevice.Viewport.Width;
                    foreach (Enemy enemy in m_Enemies)
                    {
                        m_maxY = (int)MathHelper.Max(m_maxY, enemy.Position.Y + enemy.Height);
                        enemy.NumOfJumps = (int)(m_TimeSinceMoved / m_TimeBetweenJumps);
                        enemy.Update(i_GameTime);

                        if (m_BatchMovingRight)
                        {
                            if (enemy.Position.X > m_XMax)
                            {
                                m_XMax = enemy.Position.X;
                            }
                        }
                        else
                        {
                            if (enemy.Position.X < m_XMin)
                            {
                                m_XMin = enemy.Position.X;
                            }
                        }
                    }

                    if (batchHitWall())
                    {
                        float offset = getOffset();            
                        readjustEnemyBatch(offset);
                        m_BatchMovingRight = !m_BatchMovingRight;
                        m_EnemyHitWall = true;
                    }
                }

                m_TimeSinceMoved -= m_TimeBetweenJumps;
            }
            else
            {
                updateAnimations(i_GameTime);
            }

            m_MotherShip.Update(i_GameTime);
            m_Enemies.RemoveAll(enemy => enemy.WasHit == true);   
            if (NoMoreEnemies != null && EnemyCount == 0)
            {
                NoMoreEnemies.Invoke(this, EventArgs.Empty);
            }

            if (m_maxY >= Game.GraphicsDevice.Viewport.Height - ObjectValues.SpaceshipSize)
            {
                On_ReachBottom();
            }
        }

        private void On_ReachBottom()
        {
            if (EnemyReachedBottom != null)
            {
                EnemyReachedBottom.Invoke(this, EventArgs.Empty);
            }
        }

        private void updateAnimations(GameTime i_GameTime)
        {
            foreach(Enemy enemy in m_Enemies)
            {
                enemy.Animations.Update(i_GameTime);
            }
        }

        private void speedUpEnemies()
        {
            m_TimeBetweenJumps -= m_TimeBetweenJumps * 0.04f;
            m_Enemies.ForEach(delegate(Enemy enemy)
            {
                enemy.UpdateCellSpeed(TimeSpan.FromSeconds(m_TimeBetweenJumps));
            });
        }

        private float getOffset()
        {
            float offset;
            if (m_BatchMovingRight)
            {
                offset = m_XMax - (Game.GraphicsDevice.Viewport.Width - r_EnemySize);
            }
            else
            {
                offset = m_XMin;
            }

            return offset;
        }

        private void readjustEnemyBatch(float offset)
        {
            foreach(Enemy enemy in m_Enemies)
            {
                enemy.Position = new Vector2(enemy.Position.X - offset, enemy.Position.Y);
            }
        }

        private bool batchHitWall()
        {
            return (m_XMax >= (Game.GraphicsDevice.Viewport.Width - r_EnemySize) && m_BatchMovingRight) || (m_XMin <= 0 && !m_BatchMovingRight);
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin(SpriteSortMode.Texture, BlendState.NonPremultiplied);
            foreach(Enemy enemy in m_Enemies)
            {
                enemy.Draw(gameTime);
            }

            m_MotherShip.Draw(gameTime);

            m_SpriteBatch.End();
        }

        private int GetEnemyIndexInTextureByRow(int i_Row)
        {
            int spriteLoc = 0;
            switch (i_Row)
            {
                case 0:
                    spriteLoc = r_Enemy1IndexInTexture;
                    break;
                case 1:
                case 2:
                    spriteLoc = r_Enemy2IndexInTexture;
                    break;
                case 3:
                case 4:
                    spriteLoc = r_Enemy3IndexInTexture;
                    break;
            }

            return spriteLoc;
        }

        private int GetEnemyXLocation(int i_Row)
        {
            int spriteLoc = 0;
            switch (i_Row)
            {
                case 0:
                case 1:
                case 3:
                    spriteLoc = 0;
                    break;
                default:
                    spriteLoc = (int)r_EnemySize + 1;
                    break;
            }

            return spriteLoc;
        }

        private Color GetEnemyTintByRow(int i_Row)
        {
            Color enemyTint = Color.White;
            switch (i_Row)
            {
                case 0:
                    enemyTint = r_Enemy1Tint;
                    break;
                case 1:
                case 2:
                    enemyTint = r_Enemy2Tint;
                    break;
                case 3:
                case 4:
                    enemyTint = r_Enemy3Tint;
                    break;
            }

            return enemyTint;
        }

        private int GetEnemyValueByRow(int i_Row)
        {
            int enemyVal = 0;
            switch (i_Row)
            {
                case 0:
                    enemyVal = m_Enemy1Value;
                    break;
                case 1:
                case 2:
                    enemyVal = m_Enemy2Value;
                    break;
                case 3:
                case 4:
                    enemyVal = m_Enemy3Value;
                    break;
            }

            return enemyVal;
        }
    }
}
