﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators
{
    public abstract class Animator
    {
        private DynamicDrawableComponent m_BoundComponent;
        private TimeSpan m_AnimationLength;
        private TimeSpan m_TimeLeft;
        private bool m_IsFinished = false;
        private bool m_Enabled = true;
        private bool m_Initialized = false;
        private string m_Name;
        protected bool m_ResetAfterFinish = true;
        protected internal DynamicDrawableComponent m_OriginalComponentInfo;

        public event EventHandler Finished;

        protected virtual void OnFinished()
        {
            if (m_ResetAfterFinish)
            {
                Reset();
                this.m_IsFinished = true;
            }

            if (Finished != null)
            {
                Finished(this, EventArgs.Empty);
            }
        }

        protected Animator(string i_Name, TimeSpan i_AnimationLength)
        {
            m_Name = i_Name;
            m_AnimationLength = i_AnimationLength;
        }

        protected internal DynamicDrawableComponent BoundComponent
        {
            get { return m_BoundComponent; }
            set { m_BoundComponent = value; }
        }

        public string Name
        {
            get { return m_Name; }
        }

        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        public bool IsFinite
        {
            get { return this.m_AnimationLength != TimeSpan.Zero; }
        }

        public bool ResetAfterFinish
        {
            get { return m_ResetAfterFinish; }
            set { m_ResetAfterFinish = value; }
        }

        public virtual void Initialize()
        {
            if (!m_Initialized)
            {
                m_Initialized = true;

                CloneComponentInfo();

                Reset();
            }
        }

        protected virtual void CloneComponentInfo()
        {
            if (m_OriginalComponentInfo == null)
            {
                m_OriginalComponentInfo = m_BoundComponent.ShallowClone();
            }
        }

        public void Reset()
        {
            Reset(m_AnimationLength);
        }

        public void Reset(TimeSpan i_AnimationLength)
        {
            if (!m_Initialized)
            {
                Initialize();
            }
            else
            {
                m_AnimationLength = i_AnimationLength;
                m_TimeLeft = m_AnimationLength;
                this.IsFinished = false;
            }

            RevertToOriginal();
        }

        protected abstract void RevertToOriginal();

        public void Pause()
        {
            this.Enabled = false;
        }

        public void Resume()
        {
            m_Enabled = true;
        }

        public virtual void Restart()
        {
            Restart(m_AnimationLength);
        }

        public virtual void Restart(TimeSpan i_AnimationLength)
        {
            Reset(i_AnimationLength);
            Resume();
        }

        protected TimeSpan AnimationLength
        {
            get { return m_AnimationLength; }
        }

        public bool IsFinished
        {
            get { return this.m_IsFinished; }
            protected set
            {
                if (value != m_IsFinished)
                {
                    m_IsFinished = value;
                    if (m_IsFinished == true)
                    {
                        OnFinished();
                    }
                }
            }
        }

        public void Update(GameTime i_GameTime)
        {
            if (!m_Initialized)
            {
                Initialize();
            }

            if (this.Enabled && !this.IsFinished)
            {
                if (this.IsFinite)
                {
                    // check if we should stop animating:
                    m_TimeLeft -= i_GameTime.ElapsedGameTime;

                    if (m_TimeLeft.TotalSeconds < 0)
                    {
                        this.IsFinished = true;
                    }
                }

                if (!this.IsFinished)
                {
                    // we are still required to animate:
                    DoFrame(i_GameTime);
                }
            }
        }

        protected abstract void DoFrame(GameTime i_GameTime);
    }
}