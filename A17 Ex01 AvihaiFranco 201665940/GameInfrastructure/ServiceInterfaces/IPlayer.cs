using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.ObjectModel;

namespace GameInfrastructure.ServiceInterfaces
{
    public interface IPlayer
    {
        event EventHandler<EventArgs> PlayerDead;

        event EventHandler<EventArgs> PlayerHit;
        
        DynamicDrawableComponent GameComponent { get; set; }

        int Score { get; set; }

        int Lives { get; set; }

        string PlayerId { get; set; }

    }

    public interface I2DPlayer : IPlayer
    {
        Vector2 GameComponentPosition { get; set; }

        Rectangle GameComponentBounds { get; }
    }
}
