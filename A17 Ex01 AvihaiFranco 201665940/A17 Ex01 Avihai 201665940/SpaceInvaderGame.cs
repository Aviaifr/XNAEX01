using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public class SpaceInvaderGame : Game
    {
        public static int EnemyRows = 5;
        public static int EnemyCols = 9;
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private IGameObject m_Background;
        private List<IGameObject> m_MovingObjects;
        private bool m_changeEnemyDirectionFlag;
        private float m_FixEnemyOffset;

        public SpaceInvaderGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            m_changeEnemyDirectionFlag = false;
            m_MovingObjects = new List<IGameObject>();
            
            for (int i = 0; i < EnemyRows; i++)
            {
                for (int j = 0; j < EnemyCols; j++)
                {
                    Enemy currEnemy = new Enemy(this, EnemyValues.GetEnemySpriteByRow(i), EnemyValues.GetEnemyValueByRow(i));
                    currEnemy.m_WallHit += enemyWallHitHandler;
                    m_MovingObjects.Add(currEnemy);
                }
            }

            base.Initialize();
        }

        private Vector2 getEnemyPosition(int i_row, int i_col)
        {
            float y = (3 * EnemyValues.EnemyWidth) + ((1.6f * EnemyValues.EnemyWidth) * i_row);
            float x = (1.6f * EnemyValues.EnemyWidth) * i_col;
            return new Vector2(x, y);
        }

        protected override void LoadContent()
        {
            int counter = 0;
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            m_Background = new Background(this, @"Backgrounds/BG_Space01_1024x768");
            foreach (Enemy enemy in m_MovingObjects)
            {
                enemy.Initialize(getEnemyPosition(counter / EnemyCols, counter % EnemyCols), EnemyValues.GetEnemyTintByRow(counter / EnemyCols), new Vector2());
                counter++;
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (Enemy enemy in m_MovingObjects)
            {
                enemy.Update(gameTime);
            }

            checkAndChangeDirection();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_SpriteBatch.Begin();
            m_Background.Draw(m_SpriteBatch);
            foreach (Enemy enemy in m_MovingObjects)
            {
                enemy.Draw(m_SpriteBatch);
            }

            m_SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void enemyWallHitHandler(SpaceObject i_ObjectHitTheWall, float i_XFixOffset)
        {
            m_changeEnemyDirectionFlag = true;
            m_FixEnemyOffset = i_XFixOffset;
        }

        private void checkAndChangeDirection()
        {
            if (m_changeEnemyDirectionFlag == true)
            {
                foreach (Enemy enemy in m_MovingObjects)
                {
                    enemy.ChangeDirection(m_FixEnemyOffset);
                }
            }

            m_changeEnemyDirectionFlag = false;
        }
    }
}
