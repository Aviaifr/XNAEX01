using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public delegate void ObjectFireHandler(IShootingObject i_ShootingObject);
    
    public interface IShootingObject
    {
        void Shoot();

        Vector2 GetShotStartingPosition();

        void OnMybulletDisappear(SpaceBullet i_DisappearedBullet);
    }
}
