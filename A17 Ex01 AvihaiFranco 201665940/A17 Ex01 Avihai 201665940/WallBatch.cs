using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ObjectModel.Animators;
using GameInfrastructure.ObjectModel.Animators.ConcreteAnimators;

namespace Space_Invaders
{
    class WallBatch : CompositeSprite
    {
        public WallBatch(Game i_Game)
            : base(i_Game)
        {
        }
        private readonly int r_WallCount = 4;

        public override void Initialize()
        {
            for (int i = 0; i < r_WallCount; i++)
            {
                this.Add(new Wall(this.Game, @"Barriers\Barrier_44x32"));
            }
            
            base.Initialize();

            setWallsParams();
        }

        private void setWallsParams()
        {
            int wallWidth = (int)m_SpritesList[0].Width;
            int totalWidth = wallWidth * r_WallCount * 2;
            int startingPosition = (this.Game.GraphicsDevice.Viewport.Width - totalWidth) / 2;
            bool v_LoopAnimation = true;
            m_Position.Y = Game.GraphicsDevice.Viewport.Height - ObjectValues.SpaceshipSize - (2 * m_SpritesList[0].Height); 
            for (int i = 0; i < m_SpritesList.Count; i++)
            {
                m_SpritesList[i].Position = new Vector2(startingPosition + (i * 2 * wallWidth), m_Position.Y);
                Vector2 movingPosition1 = new Vector2(m_SpritesList[i].Position.X - (wallWidth/2), m_SpritesList[i].Position.Y);
                Vector2 movingPosition2 = new Vector2(m_SpritesList[i].Position.X + (wallWidth/2), m_SpritesList[i].Position.Y);
                SpriteAnimator wpa = new Waypointsanimator(60, TimeSpan.Zero, v_LoopAnimation, movingPosition1, movingPosition2);
                wpa.Enabled = true;
                m_SpritesList[i].Animations.Add(wpa);
            }
        }
    }
}
