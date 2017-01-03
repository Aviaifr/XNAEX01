using System;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class Waypointsanimator : SpriteAnimator
    {
        private float m_VelocityPerSecond;
        private Vector2[] m_Waypoints;
        private int m_CurrentWaypointIdx = 0;
        private bool m_Loop = false;

        public Waypointsanimator(
            float i_VelocityPerSecond,
            TimeSpan i_AnimationLength,
            bool i_Loop,
            params Vector2[] i_Waypoints)

            : this("Waypoints", i_VelocityPerSecond, i_AnimationLength, i_Loop, i_Waypoints)
        {
        }

        public Waypointsanimator(
            string i_Name,
            float i_VelocityPerSecond,
            TimeSpan i_AnimationLength,
            bool i_Loop,
            params Vector2[] i_Waypoints)

            : base(i_Name, i_AnimationLength)
        {
            this.m_VelocityPerSecond = i_VelocityPerSecond;
            this.m_Waypoints = i_Waypoints;
            m_Loop = i_Loop;
            m_ResetAfterFinish = false;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Position = m_OriginalSpriteInfo.Position;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float maxDistance = (float)i_GameTime.ElapsedGameTime.TotalSeconds * m_VelocityPerSecond;

            Vector2 remainingVector = m_Waypoints[m_CurrentWaypointIdx] - this.BoundSprite.Position;
            if (remainingVector.Length() > maxDistance)
            {
                remainingVector.Normalize();
                remainingVector *= maxDistance;
            }

            this.BoundSprite.Position += remainingVector;

            if (reachedCurrentWaypoint())
            {
                lookAtNextWayPoint();
            }
        }

        private void lookAtNextWayPoint()
        {
            if (reachedLastWaypoint() && !m_Loop)
            {
                this.IsFinished = true;
            }
            else
            {
                m_CurrentWaypointIdx++;
                m_CurrentWaypointIdx %= m_Waypoints.Length;
            }
        }

        private bool reachedLastWaypoint()
        {
            return m_CurrentWaypointIdx == m_Waypoints.Length - 1;
        }

        private bool reachedCurrentWaypoint()
        {
            return this.BoundSprite.Position == m_Waypoints[m_CurrentWaypointIdx];
        }
    }
}
