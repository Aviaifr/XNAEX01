using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A17_Ex01_Avihai_201665940
{
    public interface IGameObject
    {
        void Update(GameTime i_GameTime);

        void Draw(SpriteBatch i_SpriteBatch);

        void Initialize(Vector2 i_Position, Color i_Tint, Vector2 i_Speed);
    }
}
