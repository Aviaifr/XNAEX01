using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    public static class ObjectValues
    {
        public static readonly string BackgroundTextureString = @"Backgrounds/BG_Space01_1024x768";
        public static readonly string UserShipTextureString = "Spaceship/Ship01_32x32";
        public static readonly string UserShip2TextureString = "Spaceship/Ship02_32x32";
        public static readonly string MothershipTextureString = "Enemies/MotherShip_32x120";
        public static readonly string BulletTextureString = "Shots/Bullet";
        public static readonly int EnemyWidth = 32;
        public static readonly int SpaceshipSize = 32;
        public static readonly Color MothershipTint = Color.White;
        public static readonly Color UserShipBulletTint = Color.Red;
        public static readonly Color EnemyBulletTint = Color.Blue;
        public static readonly int MothershipValue = 650;
        public static readonly int SpaceshipValue = 1200;
        public static readonly string BlinkingAnimator = "Blink";
        public static readonly string FadingAnimator = "Fade";
        public static readonly string SizeAnimator = "Size";
        public static readonly string RotateAnimator = "Rotate";
        public static readonly string HitAnimation = "HitAnimation";
        public static readonly string DeathAnimation = "DeathAnimation";
    }
}
