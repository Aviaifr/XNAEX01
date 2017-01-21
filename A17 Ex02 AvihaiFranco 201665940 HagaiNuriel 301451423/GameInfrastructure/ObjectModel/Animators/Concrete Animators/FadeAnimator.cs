using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class FadeAnimator : Animator
    {
        private float m_FadingPace;

        public FadeAnimator(string i_Name, TimeSpan i_AnimationLength) : 
            base(i_Name, i_AnimationLength)
        {
            m_FadingPace = 1 / (float)i_AnimationLength.TotalSeconds;
        }

        public FadeAnimator(TimeSpan i_AnimationLength) :
            this("Fade", i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (BoundComponent.Opacity > 0)
            {
                BoundComponent.Opacity -= m_FadingPace * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                BoundComponent.Opacity = MathHelper.Clamp(BoundComponent.Opacity, 0, 1);
            }
        }

        protected override void RevertToOriginal()
        {
            BoundComponent.Opacity = m_OriginalComponentInfo.Opacity;
        }
    }
}
