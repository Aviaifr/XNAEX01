using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class SizeAnimator : Animator
    {
        private Vector2 m_ScaleChange;

        public SizeAnimator(string i_Name, TimeSpan i_AnimationLength, e_SizeType i_SizeType) :
            base(i_Name, i_AnimationLength)
        {
            float scaleChange = i_SizeType == e_SizeType.Enlarge ? 0.3f / (float)i_AnimationLength.TotalSeconds : 
                -1 * 0.3f / (float)i_AnimationLength.TotalSeconds;

            m_ScaleChange = new Vector2(scaleChange, scaleChange);
        }

        public SizeAnimator(TimeSpan i_AnimationLength, e_SizeType i_SizeType) :
            this("Size" + i_SizeType.ToString(), i_AnimationLength, i_SizeType)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundComponent.Scales += m_ScaleChange * 
                (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            BoundComponent.Scales = m_OriginalComponentInfo.Scales;
        }
    }
}

public enum e_SizeType
{
    Shrink, Enlarge
}
