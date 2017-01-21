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
    public class ScreenOptionsScreen : MenuScreen
    {
        private ISettingsManager m_SettingsManager;
        
        public ScreenOptionsScreen(Game i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            SelectionChangeSoundEffect = Game.Content.Load<SoundEffect>(@"C:/Temp/XNA_Assets/Ex03/Sounds/MenuMove");
            TextComponent TitleTextComponent = new TextComponent(Game, "Screen Options", @"Fonts/Consolas");
            TitleTextComponent.Scale = new Vector2(4, 5);
            TitleTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 150);
            TitleTextComponent.AlignToCenter();
            Add(TitleTextComponent);
            Add(new Background(this.Game, ObjectValues.BackgroundTextureString));

            m_SettingsManager = Game.Services.GetService(typeof(ISettingsManager)) as ISettingsManager;
            SettingMenuItem MouseVisibility =
                new SettingMenuItem(Game, "Mouse Visibility", @"Fonts/Consolas", Color.Blue, Color.Red);
            MouseVisibility.ExtraText = m_SettingsManager.IsMouseVisible ? "Visible" : "Invisible";
            MouseVisibility.ToggleDown += onMouseVisibilityToggled;
            MouseVisibility.ToggleUp += onMouseVisibilityToggled;
            MouseVisibility.Scale = Vector2.One * 2f;
            SettingMenuItem WindowResizing =
                new SettingMenuItem(Game, "Allow Window Resizing", @"Fonts/Consolas", Color.Blue, Color.Red);

            WindowResizing.ExtraText = m_SettingsManager.IsResizeable ? "On" : "Off";
            WindowResizing.ToggleDown += onWindowResizingToggled;
            WindowResizing.ToggleUp += onWindowResizingToggled;
            WindowResizing.Scale = Vector2.One * 2f;
            SettingMenuItem FullScreenMode =
                 new SettingMenuItem(Game, "Full Screen Mode", @"Fonts/Consolas", Color.Blue, Color.Red);

            FullScreenMode.ExtraText = m_SettingsManager.IsFullScreen ? "On" : "Off";
            FullScreenMode.ToggleDown += onFullScreenModeToggled;
            FullScreenMode.ToggleUp += onFullScreenModeToggled;
            FullScreenMode.Scale = Vector2.One * 2f;
            ChooseableMenuItem DoneOption =
                new ChooseableMenuItem(Game, "Done", @"Fonts/Consolas", Color.Blue, Color.Red);
            DoneOption.Scale = Vector2.One * 2f;
            DoneOption.Choose += onDoneSelected;

            Add(MouseVisibility);
            Add(WindowResizing);
            Add(FullScreenMode);
            Add(DoneOption);
            base.Initialize();
        }

        private void onMouseVisibilityToggled(object i_Object, EventArgs i_EventArgs)
        {
            SettingMenuItem mouseVisibility = i_Object as SettingMenuItem;
            m_SettingsManager.ToggleMouseVisibility();
            mouseVisibility.ExtraText = m_SettingsManager.IsMouseVisible ? "Visible" : "Invisible";
        }

        private void onWindowResizingToggled(object i_Object, EventArgs i_EventArgs)
        {
            SettingMenuItem windowResizing = i_Object as SettingMenuItem;
            m_SettingsManager.ToggleWindowResizeable();
            windowResizing.ExtraText = m_SettingsManager.IsResizeable ? "On" : "Off";
        }

        private void onFullScreenModeToggled(object i_Object, EventArgs i_EventArgs)
        {
            SettingMenuItem fullScreenSetting = i_Object as SettingMenuItem;
            m_SettingsManager.ToggleFullScreen();
            fullScreenSetting.ExtraText = m_SettingsManager.IsFullScreen ? "On" : "Off";
        }

        private void onDoneSelected(object i_Object, EventArgs i_EventArgs)
        {
            this.ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}