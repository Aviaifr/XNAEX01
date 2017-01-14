using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{   
    public interface IShootingObject
    {
        void OnMyBulletDisappear(object i_SpaceBullet, EventArgs i_EventArgs);

        event EventHandler<EventArgs> Shoot;
    }
}
