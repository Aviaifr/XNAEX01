﻿using System;
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
        private const int k_EnemyInitCols = 9;
        private const int k_BarrierInitSpeed = 60;
        private const int k_EnemyFireChance = 1;
        private const int k_LevelsBeforeReset = 5;
        private const int k_Enemy1InitValue = 240;
        private const int k_Enemy2InitValue = 170;
        private const int k_Enemy3InitValue = 140;

        private int m_Level = 1;
        private PlayScreen m_GameScreen;

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
            m_Level = (m_Level + 1) % k_LevelsBeforeReset;

            if(m_Level % k_LevelsBeforeReset == 0)
            {
                ResetDifficulty();
                m_Level = 1;
            }
            else
            {
                m_GameScreen.WallBatch.Velocity *= 0.94f;
                m_GameScreen.EnemyBatch.EnemyCols += 1;
                m_GameScreen.EnemyBatch.EnemyFireChance -= 0.2f;
                m_GameScreen.EnemyBatch.IncreaseEnemyScores(70);
            }

            if (m_Level % k_LevelsBeforeReset == 2)
            {
                m_GameScreen.WallBatch.Velocity = Vector2.UnitX * k_BarrierInitSpeed;
            }   
        }

        public void ResetDifficulty()
        {
            m_GameScreen.EnemyBatch.ResetEnemyValues();
            m_GameScreen.WallBatch.Velocity = Vector2.Zero;
            m_GameScreen.EnemyBatch.EnemyCols = k_EnemyInitCols;
            m_GameScreen.EnemyBatch.EnemyFireChance = k_EnemyFireChance;
        }
    }
}