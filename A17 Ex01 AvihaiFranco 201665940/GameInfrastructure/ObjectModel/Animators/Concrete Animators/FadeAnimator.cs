using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.Concrete_Animators
{
    public class FadeAnimator : SpriteAnimator
    {
        private float m_FadingPace;

        public FadeAnimator(string i_Name, TimeSpan i_AnimationLength, float i_FadingPace) : 
            base(i_Name, i_AnimationLength)
        {
            m_FadingPace = i_FadingPace;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.Opacity += m_FadingPace * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
        }
    }
}
