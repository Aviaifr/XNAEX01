using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace A17_Ex01_Avihai_201665940
{
    public delegate void DisappearHandler(SpaceBullet i_BulletToDisappear);

    public class SpaceBullet : Sprite
    {
        public static Vector2 s_BulletSpeed = new Vector2(0, 120);
        
        public bool ToBeRemoved { get; private set; }

        public event DisappearHandler NotifyDiappeared;

        public SpaceBullet(Game i_Game, string i_TextureString)
            : base(i_Game, i_TextureString)
        {
        }

        public override void Update(GameTime i_GameTime)
        {
            m_Position.Y += m_Speed.Y * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_Position.Y >= m_Game.GraphicsDevice.Viewport.Height || (m_Position.Y <= 0 && m_Speed.Y < 0))
            {
                disappear();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Position.X -= m_Texture.Width / 2;
            if (m_Speed.Y < 0)
            {
                m_Position.Y -= m_Texture.Height;
            }
        }


        private void disappear()
        {
            if (NotifyDiappeared != null && this.ToBeRemoved == false)
            {
                NotifyDiappeared.Invoke(this);
            }

            this.ToBeRemoved = true;
        }
    }
}
