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
            (Game.Services.GetService(typeof(ISettingsManager)) as ISettingsManager).Level = 1;
            TextComponent wellcomTextComponent = new TextComponent(Game, "Wellcome To Space Invaders", @"Fonts/Consolas");
            wellcomTextComponent.Scale = new Vector2(3, 5);
            TextComponent startGameTextComponent = new TextComponent(Game,
@"Press 'Enter' To Start The Game
Press 'M' For Main Menu
Press 'Esc' To Quit",
                    @"Fonts/Consolas");
            wellcomTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 150);
            wellcomTextComponent.AlignToCenter();
            startGameTextComponent.Tint = Color.Gold;
            startGameTextComponent.Scale = Vector2.One * 2;
            startGameTextComponent.AlignToCenter();
            startGameTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 400); ;
            Add(new Background(this.Game, ObjectValues.BackgroundTextureString));
            Add(wellcomTextComponent);
            Add(startGameTextComponent);
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
                ExitScreen();
                this.ScreensManager.SetCurrentScreen(new MainMenuScreen(Game));
            }
            else if(this.InputManager.KeyPressed(Keys.Enter))
            {
                ExitScreen();
                (ScreensManager as ScreensMananger).Push(new PlayScreen(Game));
                this.ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
            }
            else if (this.InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
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
