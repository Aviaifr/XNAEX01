using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class FadeAnimator : SpriteAnimator
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
            if (BoundSprite.Opacity > 0)
            {
                BoundSprite.Opacity -= m_FadingPace * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                BoundSprite.Opacity = MathHelper.Clamp(BoundSprite.Opacity, 0, 1);
            }
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
        }
    }
}
