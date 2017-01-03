using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class RotateAnimator : SpriteAnimator
    {
        private float m_RotationsPerSecond;

        public RotateAnimator(string i_Name, TimeSpan i_AnimationLength, float i_RotationsPerSecond) : 
            base(i_Name, i_AnimationLength)
        {
            m_RotationsPerSecond = i_RotationsPerSecond * MathHelper.TwoPi;
        }

        public RotateAnimator(TimeSpan i_AnimationLength, float i_RotationsPerSecond) : 
            this("Rotate", i_AnimationLength, i_RotationsPerSecond)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.Rotation += 
                m_RotationsPerSecond * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Rotation = m_OriginalSpriteInfo.Rotation;
        }
    }
}
