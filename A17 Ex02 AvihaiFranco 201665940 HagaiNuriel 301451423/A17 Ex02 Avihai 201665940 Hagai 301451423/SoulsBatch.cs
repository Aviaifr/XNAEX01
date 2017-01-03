using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class SoulsBatch : CompositeSprite
    {
        private readonly int r_NumOfSouls = 3;

        public SoulsBatch(Game i_Game, Color i_Tint, Vector2 i_Position)
            : base(i_Game)
        {
            this.m_TintColor = i_Tint;
            this.m_Position = i_Position;
        }

        public SoulsBatch(Game i_Game, Color i_Tint, Vector2 i_Position, int i_NumOfSouls)
            : this(i_Game, i_Tint, i_Position)
        {
            r_NumOfSouls = i_NumOfSouls;
        }

        public override void Initialize()
        {
            for (int i = 0; i < r_NumOfSouls; i++)
            {
                this.Add(new Soul(this.Game, m_TintColor));
            }
            
            base.Initialize();

            setSoulsParams();
        }

        private void setSoulsParams()
        {
            for (int i = 0; i < m_SpritesList.Count; i++)
            {
                m_SpritesList[i].Position = new Vector2(m_Position.X + (i * (m_SpritesList[i].Height + (m_SpritesList[i].Height / 2))), m_Position.Y);
            }
        }

        public void RemoveSoul()
        {
            if (m_SpritesList.Count > 0)
            {
                this.m_SpritesList.RemoveRange(0, 1);
            }
        }
    }
}
