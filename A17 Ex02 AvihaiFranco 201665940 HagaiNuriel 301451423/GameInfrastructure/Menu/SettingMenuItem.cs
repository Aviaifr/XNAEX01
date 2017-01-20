using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameInfrastructure.Menu
{
    public class SettingMenuItem : MenuItem
    {
        public event EventHandler ToggleUp;
        
        public event EventHandler ToggleDown;

        public SettingMenuItem(Game i_Game, string i_Name, string i_SpriteFontLocation, Color i_ActiveTint, Color i_InActiveTint)
            : base(i_Game, i_Name, i_SpriteFontLocation, i_ActiveTint, i_InActiveTint)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            IInputManager inputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            if (isActive && inputManager.KeyPressed(Keys.PageUp))
            {
                if (ToggleUp != null)
                {
                    ToggleUp(this, EventArgs.Empty);
                }
            }

            if (isActive && inputManager.KeyPressed(Keys.PageDown))
            {
                if (ToggleDown != null)
                {
                    ToggleDown(this, EventArgs.Empty);
                }
            }

            base.Update(gameTime);
        }
    }
}
