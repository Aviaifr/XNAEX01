using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public class Background : SpaceObject
    {
        public Background(Game i_Game, string i_TextureString) : base(i_Game,i_TextureString)
        {
            Initialize(new Vector2(0, 0), Color.Gray, new Vector2(0,0));
        }

        public override void Update(GameTime i_GameTime)
        {

        }

    }
}
