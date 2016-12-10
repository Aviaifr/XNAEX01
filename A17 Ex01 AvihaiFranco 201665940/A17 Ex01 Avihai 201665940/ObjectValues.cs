using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public static class ObjectValues
    {
        public static readonly string sr_Background = @"Backgrounds/BG_Space01_1024x768";
        public static readonly string sr_UserShip = "Spaceship/Ship01_32x32";
        public static readonly string sr_Enemy1 = "Enemies/Enemy0101_32x32";
        public static readonly string sr_Enemy2 = "Enemies/Enemy0201_32x32";
        public static readonly string sr_Enemy3 = "Enemies/Enemy0301_32x32";
        public static readonly string sr_Mothership = "Enemy0301_32x32";
        public static readonly string sr_Bullet = "Shots/Bullet";
        public static readonly int sr_EnemyWidth = 32;
        public static readonly int sr_SpaceshipSize = 32;
        public static readonly Color sr_Enemy1Tint = Color.LightPink;
        public static readonly Color sr_Enemy2Tint = Color.LightBlue;
        public static readonly Color sr_Enemy3Tint = Color.LightYellow;
        public static readonly Color sr_MothershipTint = Color.White;
        public static readonly Color sr_UserShipBulletTint = Color.Red;
        public static readonly Color sr_EnemyBulletTint = Color.Blue;
        public static readonly int sr_Enemy1Value = 240;
        public static readonly int sr_Enemy2Value = 170;
        public static readonly int sr_Enemy3Value = 140;
        public static readonly int sr_MothershipValue = 650;
        public static readonly int sr_SpaceshipValue = 1200;

        public static string GetEnemySpriteByRow(int i_Row)
        {
            string spriteLoc = string.Empty;
            switch (i_Row)
            {
                case 0:
                    spriteLoc = sr_Enemy1;
                    break;
                case 1:
                case 2:
                    spriteLoc = sr_Enemy2;
                    break;
                case 3:
                case 4:
                    spriteLoc = sr_Enemy3;
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
                    enemyTint = sr_Enemy1Tint;
                    break;
                case 1:
                case 2:
                    enemyTint = sr_Enemy2Tint;
                    break;
                case 3:
                case 4:
                    enemyTint = sr_Enemy3Tint;
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
                    enemyVal = sr_Enemy1Value;
                    break;
                case 1:
                case 2:
                    enemyVal = sr_Enemy2Value;
                    break;
                case 3:
                case 4:
                    enemyVal = sr_Enemy3Value;
                    break;
            }

            return enemyVal;
        }
    }
}
