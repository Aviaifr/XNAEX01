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
    public class PauseScreen : GameScreen
    {
        public PauseScreen(Game i_Game)
            : base(i_Game)
        {
            this.IsModal = true;
            this.IsOverlayed = true;
            this.UseGradientBackground = true;
            this.BlackTintAlpha = 0.4f;
        }

        public override void Initialize()
        {
            TextComponent TitleTextComponent = new TextComponent(Game, "Game Paused", @"Fonts/Consolas");
            TitleTextComponent.Tint = Color.ForestGreen;
            TitleTextComponent.Scales = new Vector2(4, 5);
            TitleTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 150);
            TitleTextComponent.AlignToCenter();
            Add(TitleTextComponent);
            TextComponent continueTextComponent = new TextComponent(Game, "Press 'R' To Continue", @"Fonts/Consolas");
            continueTextComponent.Position = new Vector2(100, 350);
            continueTextComponent.Scales = Vector2.One * 2;
            continueTextComponent.Tint = Color.Gold;
            Add(continueTextComponent);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.R))
            {
                this.ExitScreen();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
        }
    }
}
