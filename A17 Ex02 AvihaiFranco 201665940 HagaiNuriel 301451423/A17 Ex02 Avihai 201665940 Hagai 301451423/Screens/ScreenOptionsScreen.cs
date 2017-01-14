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
    public class ScreenOptionsScreen : MenuScreen
    {
        public ScreenOptionsScreen(Game i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            SettingMenuItem MouseVisibility =
                new SettingMenuItem(Game, "Mouse Visibility", @"Fonts/Consolas", Color.Blue, Color.Red);

            MouseVisibility.ExtraText = "Invisible";
            MouseVisibility.ToggleDown += onMouseVisibilityToggled;
            MouseVisibility.ToggleUp += onMouseVisibilityToggled;

            SettingMenuItem WindowResizing =
                new SettingMenuItem(Game, "Allow Window Resizing", @"Fonts/Consolas", Color.Blue, Color.Red);

            WindowResizing.ExtraText = "Off";
            WindowResizing.ToggleDown += onWindowResizingToggled;
            WindowResizing.ToggleUp += onWindowResizingToggled;


            SettingMenuItem FullScreenMode =
                 new SettingMenuItem(Game, "Full Screen Mode", @"Fonts/Consolas", Color.Blue, Color.Red);
            FullScreenMode.ExtraText = "On";
            FullScreenMode.ToggleDown += onFullScreenModeToggled;
            FullScreenMode.ToggleUp += onFullScreenModeToggled;

            ChooseableMenuItem DoneOption =
                new ChooseableMenuItem(Game, "Done", @"Fonts/Consolas", Color.Blue, Color.Red);

            DoneOption.Choose += onDoneSelected;

            this.AddOption(MouseVisibility);
            this.AddOption(WindowResizing);
            this.AddOption(FullScreenMode);
            this.AddOption(DoneOption);
            base.Initialize();
        }

        private void onMouseVisibilityToggled(Object i_Object, EventArgs i_EventArgs)
        {
            SettingMenuItem MouseVisibility = i_Object as SettingMenuItem;
            if (MouseVisibility != null)
            {
                if (MouseVisibility.ExtraText.Contains("Visible"))
                {
                    MouseVisibility.ExtraText = "Invisible";
                }
                else
                {
                    MouseVisibility.ExtraText = "Visible";
                }

                Game.IsMouseVisible = !Game.IsMouseVisible;
            }
        }

        private void onWindowResizingToggled(Object i_Object, EventArgs i_EventArgs)
        {
            SettingMenuItem WindowResizing = i_Object as SettingMenuItem;
            if (WindowResizing != null)
            {
                if (WindowResizing.ExtraText.Contains("On"))
                {
                    WindowResizing.ExtraText = "Off";
                }
                else
                {
                    WindowResizing.ExtraText = "On";
                }

                Game.Window.AllowUserResizing = !Game.Window.AllowUserResizing;
            }
        }


        private void onFullScreenModeToggled(Object i_Object, EventArgs i_EventArgs)
        {
            SettingMenuItem FullScreenSetting = i_Object as SettingMenuItem;
            if (FullScreenSetting != null)
            {
                if (FullScreenSetting.ExtraText.Contains("On"))
                {
                    FullScreenSetting.ExtraText = "Off";
                }
                else
                {
                    FullScreenSetting.ExtraText = "On";
                }

                IGraphicsDeviceManager IGraphicsDevice = 
                    Game.Services.GetService(typeof(IGraphicsDeviceManager)) as IGraphicsDeviceManager;
                GraphicsDeviceManager GraphicsDevice = IGraphicsDevice as GraphicsDeviceManager;
                GraphicsDevice.ToggleFullScreen();

            }
        }

        private void onDoneSelected(Object i_Object, EventArgs i_EventArgs)
        {
            this.OnClosed();
        }
    }
}
