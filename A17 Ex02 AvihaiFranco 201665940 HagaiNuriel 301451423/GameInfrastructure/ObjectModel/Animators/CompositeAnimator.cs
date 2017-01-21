using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators
{
    public class CompositeAnimator : Animator
    {
        private readonly Dictionary<string, Animator> m_AnimationsDictionary =
            new Dictionary<string, Animator>();

        protected readonly List<Animator> m_AnimationsList = new List<Animator>();

        public CompositeAnimator(DynamicDrawableComponent i_BoundComponent)
            : this("AnimationsManager", TimeSpan.Zero, i_BoundComponent, new Animator[] { })
        {
            this.Enabled = false;
        }

        public CompositeAnimator(
            string i_Name,
            TimeSpan i_AnimationLength,
            DynamicDrawableComponent i_BoundComponent,
            params Animator[] i_Animations)
            : base(i_Name, i_AnimationLength)
        {
            this.BoundComponent = i_BoundComponent;

            foreach (Animator animation in i_Animations)
            {
                this.Add(animation);
            }
        }

        public void Add(Animator i_Animation)
        {
            i_Animation.BoundComponent = this.BoundComponent;
            i_Animation.Enabled = true;
            m_AnimationsDictionary.Add(i_Animation.Name, i_Animation);
            m_AnimationsList.Add(i_Animation);
        }

        public void Remove(string i_AnimationName)
        {
            Animator animationToRemove;
            m_AnimationsDictionary.TryGetValue(i_AnimationName, out animationToRemove);
            if (animationToRemove != null)
            {
                m_AnimationsDictionary.Remove(i_AnimationName);
                m_AnimationsList.Remove(animationToRemove);
            }
        }

        public void Disable(string i_AnimationName)
        {
            Animator retVal = null;
            m_AnimationsDictionary.TryGetValue(i_AnimationName, out retVal);
            if(retVal != null)
            {
                retVal.Enabled = false;
            }
        }

        public void Enable(string i_AnimationName)
        {
            Animator retVal = null;
            m_AnimationsDictionary.TryGetValue(i_AnimationName, out retVal);
            if (retVal != null)
            {
                retVal.Enabled = true;
            }
        }

        public void Reset(string i_AnimationName)
        {
            Animator retVal = null;
            m_AnimationsDictionary.TryGetValue(i_AnimationName, out retVal);
            if (retVal != null)
            {
                retVal.Reset();
            }
        }

        public void Restart(string i_AnimationName)
        {
            Animator retVal = null;
            m_AnimationsDictionary.TryGetValue(i_AnimationName, out retVal);
            if (retVal != null)
            {
                retVal.Restart();
            }
        }

        public Animator this[string i_Name]
        {
            get
            {
                Animator retVal = null;
                m_AnimationsDictionary.TryGetValue(i_Name, out retVal);
                return retVal;
            }
        }

        public override void Restart()
        {
            base.Restart();

            foreach (Animator animation in m_AnimationsList)
            {
                animation.Restart();
            }
        }

        public override void Restart(TimeSpan i_AnimationLength)
        {
            base.Restart(i_AnimationLength);

            foreach (Animator animation in m_AnimationsList)
            {
                animation.Restart();
            }
        }

        protected override void RevertToOriginal()
        {
            foreach (Animator animation in m_AnimationsList)
            {
                animation.Reset();
            }
        }

        protected override void CloneComponentInfo()
        {
            base.CloneComponentInfo();

            foreach (Animator animation in m_AnimationsList)
            {
                animation.m_OriginalComponentInfo = m_OriginalComponentInfo;
            }
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            foreach (Animator animation in m_AnimationsList)
            {
                animation.Update(i_GameTime);
            }
        }

        public virtual bool Contains(string i_AnimationName)
        {
            return m_AnimationsDictionary.ContainsKey(i_AnimationName);
        }
    }
}
