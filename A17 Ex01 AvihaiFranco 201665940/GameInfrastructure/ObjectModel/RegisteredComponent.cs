using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace GameInfrastructure.ObjectModel
{
    public class RegisteredComponent : GameComponent
    {
        public RegisteredComponent(Game i_Game, int i_UpdateOrder)
            : base(i_Game)
        {
            this.UpdateOrder = i_UpdateOrder;
            Game.Components.Add(this); // self-register as a coponent
        }

        public RegisteredComponent(Game i_Game)
            : this(i_Game, int.MaxValue)
        { }
    }
}
