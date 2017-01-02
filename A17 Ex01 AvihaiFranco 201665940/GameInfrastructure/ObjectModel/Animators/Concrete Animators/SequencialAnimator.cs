using System;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class SequencialAnimator : CompositeAnimator
    {
        public SequencialAnimator(
            string i_Name,
            TimeSpan i_AnimationLength,
            Sprite i_BoundSprite,
            params SpriteAnimator[] i_Animations)
            : base(i_Name, i_AnimationLength, i_BoundSprite, i_Animations)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            bool allFinished = true;
            foreach (SpriteAnimator animation in m_AnimationsList)
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
                this.IsFinished = true;
            }
        }
    }
}
