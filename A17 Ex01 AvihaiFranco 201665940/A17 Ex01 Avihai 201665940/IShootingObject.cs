using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{   
    public interface IShootingObject
    {
        void OnMyBulletDisappear(object i_SpaceBullet, EventArgs i_EventArgs);

        event EventHandler<EventArgs> Shoot;
    }
}
