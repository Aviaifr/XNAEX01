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
            m_MovingObjects.Add(new UserSpaceship(this, "Spaceship/Ship01_32x32"));
            createEnemies();

            base.Initialize();
        }

        private void createEnemies()
        {
            for (int i = 0; i < EnemyRows; i++)
            {
                for (int j = 0; j < EnemyCols; j++)
                {
                    Enemy currEnemy = new Enemy(this, ObjectValues.GetEnemySpriteByRow(i), ObjectValues.GetEnemyValueByRow(i));
                    currEnemy.OnWallHit += enemyWallHitHandler;
                    m_MovingObjects.Add(currEnemy);
                }
            }
        }

        private Vector2 getEnemyPosition(int i_row, int i_col)
        {
            float y = (3 * ObjectValues.EnemyWidth) + ((1.6f * ObjectValues.EnemyWidth) * i_row);
            float x = (1.6f * ObjectValues.EnemyWidth) * i_col;
            return new Vector2(x, y);
        }

        protected override void LoadContent()
        {
            int counter = 0;
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            m_Background = new Background(this, @"Backgrounds/BG_Space01_1024x768");
            foreach (IGameObject gameObject in m_MovingObjects)
            {
                if (gameObject is Enemy)
                {
                    gameObject.Initialize(getEnemyPosition(counter / EnemyCols, counter % EnemyCols), ObjectValues.GetEnemyTintByRow(counter / EnemyCols), new Vector2());
                    counter++;
                }
                else
                {
                    gameObject.Initialize(new Vector2(0, GraphicsDevice.Viewport.Height - (ObjectValues.SpaceshipSize * 2)), Color.White, Vector2.Zero);  
                }
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (IGameObject gameObject in m_MovingObjects)
            {
                gameObject.Update(gameTime);
            }

            checkAndChangeDirection();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_SpriteBatch.Begin();
            m_Background.Draw(m_SpriteBatch);
            foreach (IGameObject gameObject in m_MovingObjects)
            {
                gameObject.Draw(m_SpriteBatch);
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
                foreach (IGameObject gameObject in m_MovingObjects)
                {
                    if (gameObject is Enemy)
                    {
                        (gameObject as Enemy).ChangeDirection(m_FixEnemyOffset);
                    }
                }
            }

            m_changeEnemyDirectionFlag = false;
        }
    }
}
