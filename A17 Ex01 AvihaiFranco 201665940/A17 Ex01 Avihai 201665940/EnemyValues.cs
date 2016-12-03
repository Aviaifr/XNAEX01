using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public static class EnemyValues
    {
        public static string Enemy1 = "Enemies/Enemy0101_32x32";
        public static string Enemy2 = "Enemies/Enemy0201_32x32";
        public static string Enemy3 = "Enemies/Enemy0301_32x32";
        public static string Mothership = "Enemy0301_32x32";
        public static int EnemyWidth = 32;
        public static Color Enemy1Tint = Color.LightPink;
        public static Color Enemy2Tint = Color.LightBlue;
        public static Color Enemy3Tint = Color.LightYellow;
        public static Color MothershipTint = Color.White;
        public static int Enemy1Value = 240;
        public static int Enemy2Value = 170;
        public static int Enemy3Value = 140;
        public static int MothershipValue = 650;

        public static string GetEnemySpriteByRow(int i_Row)
        {
            string spriteLoc = string.Empty;
            switch (i_Row)
            {
                case 0:
                    spriteLoc = Enemy1;
                    break;
                case 1:
                case 2:
                    spriteLoc = Enemy2;
                    break;
                case 3:
                case 4:
                    spriteLoc = Enemy3;
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
