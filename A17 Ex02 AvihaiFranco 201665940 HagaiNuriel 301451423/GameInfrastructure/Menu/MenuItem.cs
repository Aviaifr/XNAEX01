using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ObjectModel.Animators.ConcreteAnimators;
using GameInfrastructure.Menu;

namespace GameInfrastructure.Menu
{
    public class MenuItem : TextComponent
    {
        public event EventHandler Choose;
        
        private Color m_ActiveTint;

        private Color m_InActiveTint;

        private bool m_IsActive;

        public bool isActive
        {
            get
            {
                return m_IsActive;
            }

            set
            {
                bool preval = m_IsActive;
                m_IsActive = value;
                if (preval != m_IsActive)
                {
                    onActiveChange();
                }
            }
        }
        
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
            m_IsActive = false;
        }

        public override void Update(GameTime gameTime)
        {
            this.Animations.Update(gameTime);
            base.Update(gameTime);
        }

        private void onActiveChange()
        {
            if (m_Animations != null)
            {
                if (isActive)
                {
                    m_Animations.Enabled = true;
                }
                else
                {
                    m_Animations.Enabled = false;
                    m_Animations.Reset();
                }
            }
        }

        protected override void setupAnimation()
        {
            m_Animations.ResetAfterFinish = true;
            SizeAnimator enlarge = new SizeAnimator(TimeSpan.FromSeconds(0.5f), e_SizeType.Enlarge);
            enlarge.ResetAfterFinish = false;
            SizeAnimator shrink = new SizeAnimator(TimeSpan.FromSeconds(0.5), e_SizeType.Shrink);
            SequencialAnimator sequencial = new SequencialAnimator("beating", TimeSpan.Zero, this, enlarge, shrink);
            sequencial.Enabled = false;
            m_Animations.Add(sequencial);
            base.setupAnimation();
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
