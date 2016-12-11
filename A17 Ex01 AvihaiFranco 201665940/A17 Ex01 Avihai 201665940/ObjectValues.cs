using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public static class ObjectValues
    {
        public static readonly string BackgroundTextureString = @"Backgrounds/BG_Space01_1024x768";
        public static readonly string UserShipTextureString = "Spaceship/Ship01_32x32";
        public static readonly string Enemy1TextureString = "Enemies/Enemy0101_32x32";
        public static readonly string Enemy2TextureString = "Enemies/Enemy0201_32x32";
        public static readonly string Enemy3TextureString = "Enemies/Enemy0301_32x32";
        public static readonly string MothershipTextureString = "Enemies/MotherShip_32x120";
        public static readonly string BulletTextureString = "Shots/Bullet";
        public static readonly int EnemyWidth = 32;
        public static readonly int SpaceshipSize = 32;
        public static readonly Color Enemy1Tint = Color.LightPink;
        public static readonly Color Enemy2Tint = Color.LightBlue;
        public static readonly Color Enemy3Tint = Color.LightYellow;
        public static readonly Color MothershipTint = Color.White;
        public static readonly Color UserShipBulletTint = Color.Red;
        public static readonly Color EnemyBulletTint = Color.Blue;
        public static readonly int Enemy1Value = 240;
        public static readonly int Enemy2Value = 170;
        public static readonly int Enemy3Value = 140;
        public static readonly int MothershipValue = 650;
        public static readonly int SpaceshipValue = 1200;

        public static string GetEnemySpriteByRow(int i_Row)
        {
            string spriteLoc = string.Empty;
            switch (i_Row)
            {
                case 0:
                    spriteLoc = Enemy1TextureString;
                    break;
                case 1:
                case 2:
                    spriteLoc = Enemy2TextureString;
                    break;
                case 3:
                case 4:
                    spriteLoc = Enemy3TextureString;
                    break;
            }

            return spriteLoc;
        }

        public static Color GetEnemyTintByRow(int i_Row)
        {
            Color enemyTint = Color.White;
            switch (i_Row)
            {
                case 0:
                    enemyTint = Enemy1Tint;
                    break;
                case 1:
                case 2:
                    enemyTint = Enemy2Tint;
                    break;
                case 3:
                case 4:
                    enemyTint = Enemy3Tint;
                    break;
            }

            return enemyTint;
        }

        public static int GetEnemyValueByRow(int i_Row)
        {
            int enemyVal = 0;
            switch (i_Row)
            {
                case 0:
                    enemyVal = Enemy1Value;
                    break;
                case 1:
                case 2:
                    enemyVal = Enemy2Value;
                    break;
                case 3:
                case 4:
                    enemyVal = Enemy3Value;
                    break;
            }

            return enemyVal;
        }
    }
}
