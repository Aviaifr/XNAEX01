using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Screens;
using GameInfrastructure.Menu;

namespace Space_Invaders.Screens
{
    public class WellcomeScreen : GameScreen
    {
        public WellcomeScreen(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            TextComponent wellcomTextComponent = new TextComponent(Game, "Wellcome To Space Invaders", @"Fonts/Consolas");
            wellcomTextComponent.Scale = new Vector2(3, 5);
            TextComponent startGameTextComponent = new TextComponent(Game, "Press 'Enter' To Start The Game", @"Fonts/Consolas");
            TextComponent mainMenuTextComponent = new TextComponent(Game, "Press 'm' For Main Menu", @"Fonts/Consolas");
            TextComponent closeGameTextComponent = new TextComponent(Game, "Press 'Esc' To Quit", @"Fonts/Consolas");
            wellcomTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 200);
            mainMenuTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 300);
            startGameTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 340);
            closeGameTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 380);
            wellcomTextComponent.AlignToCenter();
            mainMenuTextComponent.AlignToCenter();
            startGameTextComponent.AlignToCenter();
            closeGameTextComponent.AlignToCenter();
            Add(wellcomTextComponent);
            Add(mainMenuTextComponent);
            Add(startGameTextComponent);
            Add(closeGameTextComponent);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.InputManager.KeyPressed(Keys.M))
            {
                this.ScreensManager.Remove(this);
                this.ScreensManager.Add(new LevelTransitionScreen(Game));
                this.ScreensManager.SetCurrentScreen(new MainMenuScreen(Game));
            }
            else if(this.InputManager.KeyPressed(Keys.Enter))
            {
                this.ScreensManager.Remove(this);
                this.ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
            }
            else if (this.InputManager.KeyPressed(Keys.Escape))
            {
                this.ExitScreen();
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
