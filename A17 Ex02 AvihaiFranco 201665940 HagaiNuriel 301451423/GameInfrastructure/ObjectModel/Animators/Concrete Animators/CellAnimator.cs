using System;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class CellAnimator : Animator
    {
        private readonly int r_NumOfCells = 1;
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrCellIdx = 0;

        public CellAnimator(TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength, int i_StartinIndex)
            : base("Cell", i_AnimationLength)
        {
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            this.r_NumOfCells = i_NumOfCells;
            this.m_CurrCellIdx = i_StartinIndex;
            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        public TimeSpan CellTime
        {
            get
            {
                return m_CellTime;
            }

            set { m_CellTime = value; }
        }

        private void goToNextFrame()
        {
            m_CurrCellIdx++;
            if (m_CurrCellIdx >= r_NumOfCells)
            {
                if (m_Loop)
                {
                    m_CurrCellIdx = 0;
                }
                else
                {
                    m_CurrCellIdx = r_NumOfCells - 1; /// lets stop at the last frame
                    this.IsFinished = true;
                }
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundComponent.SourceRectangle = m_OriginalComponentInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    goToNextFrame();
                    m_TimeLeftForCell += m_CellTime;
                }
            }

            this.BoundComponent.SourceRectangle = new Rectangle(
                m_CurrCellIdx * this.BoundComponent.SourceRectangle.Width,
                this.BoundComponent.SourceRectangle.Top,
                this.BoundComponent.SourceRectangle.Width,
                this.BoundComponent.SourceRectangle.Height);
        }
    }
}
