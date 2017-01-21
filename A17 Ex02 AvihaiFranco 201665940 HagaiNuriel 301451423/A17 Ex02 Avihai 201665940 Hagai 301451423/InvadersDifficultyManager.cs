using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Screens;
using Space_Invaders.Screens;

namespace Space_Invaders
{
    public class InvadersDifficultyManager : IDifficultyManager
    {
        //Initial Const Values
        private const int k_EnemyInitCols = 9;
        private const int k_BarrierInitSpeed = 0;
        private const int k_EnemyFireChance = 1;
        private const int k_LevelsBeforeReset = 5;
        private const int k_Enemy1InitValue = 240;
        private const int k_Enemy2InitValue = 170;
        private const int k_Enemy3InitValue = 140;

        private int m_Level = 1;
        private PlayScreen m_GameScreen;
        //private Vector2 m_BarrierSpeed;
        //private int m_EnemyCols;

        public InvadersDifficultyManager(GameScreen i_GameScreen)
        {
            m_GameScreen = i_GameScreen as PlayScreen;
        }

        public GameScreen GameToMonitor
        {
            get { return m_GameScreen; }

            set { m_GameScreen = value as PlayScreen; }
        }

        public void IncreaseDifficulty()
        {
            m_Level++;

            if(m_Level % k_LevelsBeforeReset == 0)
            {
                ResetDifficulty();
            }
            else
            {
                m_GameScreen.WallBatch.Velocity *= 0.94f;
                m_GameScreen.EnemyBatch.EnemyCols += 1;
                m_GameScreen.EnemyBatch.EnemyFireChance -= 0.2f;
                m_GameScreen.EnemyBatch.IncreaseEnemyScores(70);
            }
        }

        public void ResetDifficulty()
        {
            m_GameScreen.EnemyBatch.ResetEnemyValues();
            m_GameScreen.WallBatch.Velocity = Vector2.UnitX * k_BarrierInitSpeed;
            m_GameScreen.EnemyBatch.EnemyCols = k_EnemyInitCols;
            m_GameScreen.EnemyBatch.EnemyFireChance = k_EnemyFireChance;
        }
    }
}
