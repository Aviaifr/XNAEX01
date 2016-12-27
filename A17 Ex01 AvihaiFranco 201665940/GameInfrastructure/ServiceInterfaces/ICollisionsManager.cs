using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameInfrastructure.ServiceInterfaces
{
    public interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }

    public interface ICollidable
    {
        event EventHandler<EventArgs> PositionChanged;

        event EventHandler<EventArgs> SizeChanged;

        event EventHandler<EventArgs> VisibleChanged;

        event EventHandler<EventArgs> Disposed;

        bool IsVisible { get; }

        bool IsCollidedWith(ICollidable i_Collidable);

        void Collided(ICollidable i_Collidable);
    }

    public interface ICollidable2D : ICollidable
    {
        Rectangle Bounds { get; set; }

        Vector2 Velocity { get; set; }

        bool IsPixelBasedCollision(ICollidable i_Source);

        bool IsPointInScreenIsColidablePixel(Point i_PointOnScreen);

        bool CanCollideWith(ICollidable i_Source);
    }

    public interface ICollidable3D : ICollidable
    {
        BoundingBox Bounds { get; }

        Vector3 Velocity { get; }
    }
}
