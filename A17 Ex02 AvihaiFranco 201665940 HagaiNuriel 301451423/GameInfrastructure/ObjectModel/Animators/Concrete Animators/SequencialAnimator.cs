using System;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class SequencialAnimator : CompositeAnimator
    {
        public SequencialAnimator(
            string i_Name,
            TimeSpan i_AnimationLength,
            DynamicDrawableComponent i_BoundComponent,
            params Animator[] i_Animations)
            : base(i_Name, i_AnimationLength, i_BoundComponent, i_Animations)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            bool allFinished = true;
            foreach (Animator animation in m_AnimationsList)
            {
                if (!animation.IsFinished)
                {
                    animation.Update(i_GameTime);
                    allFinished = false;
                    break;
                }
            }

            if (allFinished)
            {
                if (!IsFinite)
                {
                    foreach(Animator animation in m_AnimationsList)
                    {
                        animation.Restart();
                    }
                }
                else
                {
                    this.IsFinished = true;
                }
            }
        }
    }
}
