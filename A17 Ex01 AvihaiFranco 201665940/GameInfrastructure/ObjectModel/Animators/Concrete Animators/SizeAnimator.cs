using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.Concrete_Animators
{
    public class SizeAnimator : SpriteAnimator
    {

        private Vector2 m_ScaleChange;

        public SizeAnimator(string i_Name, TimeSpan i_AnimationLength, Vector2 i_ScaleChange) :
            base(i_Name, i_AnimationLength)
        {
            m_ScaleChange = i_ScaleChange;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.Scales += m_ScaleChange * 
                (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Scales = m_OriginalSpriteInfo.Scales;
        }
    }
}
