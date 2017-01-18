using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.Menu;
using GameInfrastructure.ServiceInterfaces;

namespace GameInfrastructure.ObjectModel.Screens
{
    public abstract class MenuScreen : GameScreen
    {
        private List<MenuItem> m_MenuItems;
        private Vector2 m_MenuStartDrawPosition;
        private int m_CurrentIndex;
        protected SoundEffect SelectionChangeSoundEffect { get; set; }

        public MenuScreen(Game i_Game):base(i_Game)
        {
            m_CurrentIndex = 0;
            m_MenuItems = new List<MenuItem>();
            m_MenuStartDrawPosition = new Vector2(100, 200);
        }

        public void AddOption(MenuItem i_MenuItem)
        {
            m_MenuItems.Add(i_MenuItem);
            i_MenuItem.Position = new Vector2(m_MenuStartDrawPosition.X,m_MenuStartDrawPosition.Y  + m_MenuItems.Count * 30);
            Add(i_MenuItem);
        }
        
        public override void Update(GameTime gameTime)
        {
            IInputManager inputManager = (Game.Services.GetService(typeof(IInputManager)) as IInputManager);
            if (inputManager.KeyPressed(Keys.Up))
            {
                m_MenuItems[m_CurrentIndex].isActive = false;
                m_CurrentIndex--;
                m_CurrentIndex = m_CurrentIndex < 0 ? m_MenuItems.Count - 1 : m_CurrentIndex;
                itemChanged();
            }
            
            if (inputManager.KeyPressed(Keys.Down))
            {
                m_MenuItems[m_CurrentIndex].isActive = false;
                m_CurrentIndex = (m_CurrentIndex + 1) % m_MenuItems.Count;
                itemChanged();
            }

            m_MenuItems[m_CurrentIndex].isActive = true;
            base.Update(gameTime);
        }

        protected virtual void itemChanged()
        {
            if (SelectionChangeSoundEffect != null)
            {
                ISoundEffectsPlayer soundEffectsPlayer = Game.Services.GetService(typeof(ISoundEffectsPlayer)) as ISoundEffectsPlayer;
                if(soundEffectsPlayer != null)
                {
                    soundEffectsPlayer.PlaySoundEffect(SelectionChangeSoundEffect);
                }
            }
            
        }
    }
}
