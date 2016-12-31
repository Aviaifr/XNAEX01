using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.Concrete_Animators
{
    public class RotateAnimator : SpriteAnimator
    {
        private float m_RotationsPerSecond;

        public RotateAnimator(String i_Name, TimeSpan i_AnimationLength, float i_RotationsPerSecond) : 
            base(i_Name,i_AnimationLength)
        {
            m_RotationsPerSecond = i_RotationsPerSecond;
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
