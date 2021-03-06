﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Screens;

namespace Space_Invaders.Screens
{
    public class PlayScreen : GameScreen
    {
        public static int EnemyRows = 5;
        public static int EnemyCols = 9;
        private List<SpaceShipPlayer> m_Players = new List<SpaceShipPlayer>();
        private Background m_Background;
        private EnemyBatch m_EnemyBatch;

        public PlayScreen(Game i_Game) : base(i_Game)
        {
        }

        private int PointsCollected
        {
            get;
            set; 
        }

        public void Enemy_OnKill(object i_EnemyKilled, EventArgs i_eventArgs)
        {
            SpaceShipPlayer player = m_Players.Find(spaceShipPlayer =>
                (spaceShipPlayer.GameComponent as UserSpaceship) == (i_EnemyKilled as Enemy).KilledBy);
            (Game.Services.GetService(typeof(ISoundEffectsPlayer)) as ISoundEffectsPlayer).PlaySoundEffect((i_EnemyKilled as Enemy).GetSound("hit"));
            if (player != null)
            {
                player.Score += (i_EnemyKilled as Enemy).Value;
                if(i_EnemyKilled is MothershipEnemy)
                {
                    MothershipEnemy motherShip = i_EnemyKilled as MothershipEnemy;
                    motherShip.Velocity = Vector2.Zero;
                    motherShip.ActivateAnimation(ObjectValues.DeathAnimation);
                }
            }
        }

        public void enemy_OnDisposed(object i_Disposed, EventArgs i_EventArgs)
        {
            if (m_EnemyBatch.EnemyCount == 0)
            {
                GameOver();
            }
        }

        public void Player_OnHit(object i_HitPlayer, EventArgs i_EventArgs)
        {
            SpaceShipPlayer player = i_HitPlayer as SpaceShipPlayer;
            enablePlayerSpaceshipAnimation(player);
            (Game.Services.GetService(typeof(ISoundEffectsPlayer)) as ISoundEffectsPlayer).PlaySoundEffect((player.GameComponent as UserSpaceship).GetSound("hit"));
        }

        private void enablePlayerSpaceshipAnimation(SpaceShipPlayer i_Player)
        {
            Sprite playerComponent = i_Player.GameComponent as Sprite;
            playerComponent.ActivateAnimation(ObjectValues.HitAnimation);
            playerComponent.Position = (playerComponent as UserSpaceship).BeginningPosition;
        }

        public void Player_OnKilled(object i_HitPlayer, EventArgs i_EventArgs)
        {
            SpaceShipPlayer player = i_HitPlayer as SpaceShipPlayer;
            Sprite playerSprite = player.GameComponent as Sprite;

            if (playerSprite != null && playerSprite.isCollidable)
            {
                playerSprite.isCollidable = false;
                playerSprite.Animations.Enable(ObjectValues.DeathAnimation);
            }
        }

        private void checkGameOver()
        {
            bool isGameOver = true;
            foreach (Player player in m_Players)
            {
                if (player.Lives > 0)
                {
                    isGameOver = false;
                }
            }

            if (isGameOver == true)
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            ExitScreen();
        }

        public override void Initialize()
        {
            m_Background = new Background(this.Game, ObjectValues.BackgroundTextureString);
            this.Add(m_Background);
            (Game.Services.GetService(typeof(ICollisionsManager)) as ICollisionsManager).ClearCollidable();
            initPlayers();
            m_EnemyBatch = new EnemyBatch(this.Game);
            m_EnemyBatch.EnemyKilled += Enemy_OnKill;
            m_EnemyBatch.EnemyReachedBottom += Enemy_OnReachBottom;
            m_EnemyBatch.NoMoreEnemies += enemy_OnDisposed;
            this.Add(m_EnemyBatch);

            WallBatch wallBatch = new WallBatch(this.Game);
            this.Add(wallBatch);

            MothershipEnemy mothershipEnemy = new MothershipEnemy(this.Game, ObjectValues.MothershipTextureString, ObjectValues.MothershipValue);
            mothershipEnemy.Position = new Vector2(0, ObjectValues.EnemyWidth);
            mothershipEnemy.MothershipKilled += Enemy_OnKill;
            this.Add(mothershipEnemy);

            base.Initialize();
        }

        private void initPlayers()
        {
            int numOfPlayers = (Game.Services.GetService(typeof(ISettingsManager)) as ISettingsManager).NumOfPlayers;
            Vector2 startingPosition = 
                new Vector2(0, this.GraphicsDevice.Viewport.Height - ObjectValues.SpaceshipSize);
            Vector2 scorePosition = new Vector2(5, 20);
            SpaceShipPlayer player;
            UserSpaceship spaceShip;
            ScoreBoard scoreBoard;
            for(int i = 0; i < numOfPlayers; i++)
            {
                spaceShip =
                    new UserSpaceship(this.Game, ObjectValues.SpaceShipTextures[i], ObjectValues.PlayerIds[i], startingPosition);
                spaceShip.DeathAnimationFinished += OnSpaceshipDeathAnimationOver;
                spaceShip.Position = startingPosition;
                spaceShip.Shoot += spaceship_Shot;
                this.Add(spaceShip);

                player = new SpaceShipPlayer(spaceShip, ObjectValues.PlayerIds[i]);
                player.PlayerHit += Player_OnHit;
                player.PlayerDead += Player_OnKilled;
                (Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager).ClearPlayers();
                (Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager).AddPlayer(player);

                string scoreBoardText = "P" + (i + 1) + " Score: ";
                scoreBoard = new ScoreBoard(this.Game, scoreBoardText, ObjectValues.ConsolasFont);
                scoreBoard.Position = scorePosition;
                scoreBoard.Tint = ObjectValues.ScoreBoardsColors[i];
                player.ScoreBoard = scoreBoard;
                this.Add(scoreBoard);

                m_Players.Add(player); 
                startingPosition.X += ObjectValues.SpaceshipSize;
                scorePosition.Y += 20;
                SoulsBatch playerSouls = new SoulsBatch(this.Game, ObjectValues.SoulsColors[i], new Vector2(GraphicsDevice.Viewport.Width - 80, 20 * (i + 1)));
                this.Add(playerSouls);
                m_Players[i].SoulBatch = playerSouls;
            }
        }

        public void OnSpaceshipDeathAnimationOver(object i_Sender, EventArgs i_EventArgs)
        {
            checkGameOver();
        }

        private Vector2 getEnemyPosition(int i_row, int i_col)
        {
            float y = (3 * ObjectValues.EnemyWidth) + ((1.6f * ObjectValues.EnemyWidth) * i_row);
            float x = (1.6f * ObjectValues.EnemyWidth) * i_col;
            return new Vector2(x, y);
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
        
        private void spaceship_Shot(object i_Sender, EventArgs i_EventArgs)
        {
            SpaceBullet newBullet = new SpaceBullet(this.Game, ObjectValues.BulletTextureString, ObjectValues.UserShipBulletTint, -1);
            (Game.Services.GetService(typeof(ISoundEffectsPlayer)) as ISoundEffectsPlayer).PlaySoundEffect((i_Sender as UserSpaceship).GetSound("shoot"));
            newBullet.Initialize();
            newBullet.Owner = i_Sender as UserSpaceship;
            setNewSpaceshipBulletPosition(i_Sender as UserSpaceship, newBullet);
            newBullet.Disposed += (i_Sender as UserSpaceship).OnMyBulletDisappear;
            newBullet.Disposed += onComponentDisposed;
            this.Add(newBullet);
        }

        private void setNewSpaceshipBulletPosition(UserSpaceship i_Spaceship, SpaceBullet i_newBullet)
        {
            Vector2 newBulletPos = i_Spaceship.Position;
            newBulletPos.X += i_Spaceship.Width / 2;
            newBulletPos.X -= i_newBullet.Width / 2;
            newBulletPos.Y -= i_newBullet.Height;
            i_newBullet.Position = newBulletPos;
        }

        public void onComponentDisposed(object i_Disposed, EventArgs i_EventArgs)
        {
            if (this.Count > 0)
            {
                this.Remove(i_Disposed as IGameComponent);
            }
        }

        public void Enemy_OnReachBottom(object i_ReacedBottomObj, EventArgs i_EventArgs)
        {
            GameOver();
        }
    }
}
