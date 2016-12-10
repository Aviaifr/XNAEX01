using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public static class ObjectValues
    {
        public static readonly string r_Background = @"Backgrounds/BG_Space01_1024x768";
        public static readonly string r_UserShip = "Spaceship/Ship01_32x32";
        public static readonly string r_Enemy1 = "Enemies/Enemy0101_32x32";
        public static readonly string r_Enemy2 = "Enemies/Enemy0201_32x32";
        public static readonly string r_Enemy3 = "Enemies/Enemy0301_32x32";
        public static readonly string r_Mothership = "Enemy0301_32x32";
        public static readonly int r_EnemyWidth = 32;
        public static readonly int r_SpaceshipSize = 32;
        public static readonly Color r_Enemy1Tint = Color.LightPink;
        public static readonly Color r_Enemy2Tint = Color.LightBlue;
        public static readonly Color r_Enemy3Tint = Color.LightYellow;
        public static readonly Color r_MothershipTint = Color.White;
        public static readonly Color r_UserShipBulletTint = Color.Red;
        public static readonly Color r_EnemyBulletTint = Color.Blue;
        public static readonly int r_Enemy1Value = 240;
        public static readonly int r_Enemy2Value = 170;
        public static readonly int r_Enemy3Value = 140;
        public static readonly int r_MothershipValue = 650;

        public static string GetEnemySpriteByRow(int i_Row)
        {
            string spriteLoc = string.Empty;
            switch (i_Row)
            {
                case 0:
                    spriteLoc = r_Enemy1;
                    break;
                case 1:
                case 2:
                    spriteLoc = r_Enemy2;
                    break;
                case 3:
                case 4:
                    spriteLoc = r_Enemy3;
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
                    enemyTint = r_Enemy1Tint;
                    break;
                case 1:
                case 2:
                    enemyTint = r_Enemy2Tint;
                    break;
                case 3:
                case 4:
                    enemyTint = r_Enemy3Tint;
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
                    enemyVal = r_Enemy1Value;
                    break;
                case 1:
                case 2:
                    enemyVal = r_Enemy2Value;
                    break;
                case 3:
                case 4:
                    enemyVal = r_Enemy3Value;
                    break;
            }

            return enemyVal;
        }
    }
}
