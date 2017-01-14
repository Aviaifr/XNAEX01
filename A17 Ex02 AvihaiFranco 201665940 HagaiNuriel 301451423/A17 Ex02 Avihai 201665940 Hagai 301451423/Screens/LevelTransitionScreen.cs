using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Screens;
using GameInfrastructure.Menu;

namespace Space_Invaders.Screens
{
    public class LevelTransitionScreen : GameScreen
    {
        int m_Level;
        float m_TimeToStart;
        TextComponent m_StartingIn;
        public LevelTransitionScreen(Game i_Game, int i_Level)
            : base(i_Game)
        {
            m_Level = i_Level;
            m_TimeToStart = 3f;
            
        }

        public override void Initialize()
        {
            TextComponent Level = new TextComponent(Game, "Level " + m_Level, @"Fonts/Consolas");
            Level.Tint = Color.PapayaWhip;
            Level.Position = new Vector2(100, 200);
            Level.Size = 6;
            Add(Level);
            m_StartingIn = new TextComponent(Game, "Starting in ", @"Fonts/Consolas");
            m_StartingIn.Tint = Color.PapayaWhip;
            m_StartingIn.Position = new Vector2(100, 400);
            m_StartingIn.Size = 2;
            m_StartingIn.ExtraText = ((int)Math.Round(m_TimeToStart)).ToString();
            Add(m_StartingIn);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            m_TimeToStart -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_StartingIn.ExtraText = ((int)Math.Round(m_TimeToStart)).ToString();
            if (m_TimeToStart <= 0)
            {
                this.OnClosed();
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

    }
    
}
