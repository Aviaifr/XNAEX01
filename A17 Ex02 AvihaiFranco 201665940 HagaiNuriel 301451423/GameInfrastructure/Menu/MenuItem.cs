using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.Menu;

namespace GameInfrastructure.Menu
{
    public class MenuItem : TextComponent
    {
        public event EventHandler Choose;
        
        private Color m_ActiveTint;

        private Color m_InActiveTint;
        
        public bool isActive { get; set; }
        
        public override Color Tint
        {
            get
            {
                return isActive ? m_ActiveTint : m_InActiveTint;
            }
        }
        
        public MenuItem(Game i_Game, string i_Text, string i_SpriteFontLocation, Color i_ActiveTint, Color i_InActiveTint)
            : base(i_Game, i_Text, i_SpriteFontLocation)
        {
            m_ActiveTint = i_ActiveTint;
            m_InActiveTint = i_InActiveTint;
            isActive = false;
        }

        protected void onChooseOption()
        {
            if (Choose != null && isActive)
            {
                Choose(this, EventArgs.Empty);
            }
        }
    }
}
