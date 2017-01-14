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
    public class ChooseableMenuItem : MenuItem
    {
        public ChooseableMenuItem(Game i_Game, string i_Name, string i_SpriteFontLocation, Color i_ActiveTint, Color i_InActiveTint)
            : base(i_Game,i_Name,i_SpriteFontLocation,i_ActiveTint,i_InActiveTint)
        {

        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            IInputManager inputManager = (Game.Services.GetService(typeof(IInputManager)) as IInputManager);
            if (isActive && inputManager.KeyPressed(Keys.Enter))
            {
                onChooseOption();
            }
            base.Update(gameTime);
        }
    }
}
