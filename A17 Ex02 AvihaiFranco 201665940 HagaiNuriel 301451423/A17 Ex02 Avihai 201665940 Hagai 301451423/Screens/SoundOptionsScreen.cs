﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.ObjectModel.Screens;
using GameInfrastructure.Menu;

namespace Space_Invaders.Screens
{
    public class SoundOptionsScreen : MenuScreen
    {
        private Background m_Background;
        private ISettingsManager m_SettingsManager;
        private SettingMenuItem m_ToggleSound;

        public SoundOptionsScreen(Game i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            SettingMenuItem SoundFXOption;
            SettingMenuItem MusicOption;
            TextComponent TitleTextComponent = new TextComponent(Game, "Sounds Options", @"Fonts/Consolas");
            TitleTextComponent.Scale = new Vector2(4, 5);
            TitleTextComponent.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 150);
            TitleTextComponent.AlignToCenter();
            Add(TitleTextComponent);
            SelectionChangeSoundEffect = Game.Content.Load<SoundEffect>((@"C:/Temp/XNA_Assets/Ex03/Sounds/MenuMove"));
            m_SettingsManager = Game.Services.GetService(typeof(ISettingsManager)) as ISettingsManager;
            m_Background = new Background(this.Game, ObjectValues.BackgroundTextureString);
            this.Add(m_Background);
            SoundFXOption = new SettingMenuItem(Game, "Sounds Effects Volume", @"Fonts/Consolas", Color.Blue, Color.Red);
            SoundFXOption.ExtraText = Math.Round(m_SettingsManager.SoundFXVolume * 100).ToString();
            SoundFXOption.ToggleUp += onSFXVolumeUp;
            SoundFXOption.ToggleDown += onSFXVolumeDown;
            SoundFXOption.Scale = Vector2.One * 2f;
            MusicOption = new SettingMenuItem(Game, "Background Music Volume", @"Fonts/Consolas", Color.Blue, Color.Red);
            MusicOption.ExtraText = Math.Round(m_SettingsManager.BGMusicVolume * 100).ToString();
            MusicOption.ToggleUp += onBGMusicVolumeUp;
            MusicOption.ToggleDown += onBGMusicVolumeDown;
            MusicOption.Scale = Vector2.One * 2f;
            m_ToggleSound = new SettingMenuItem(Game, "Toggle Sound", @"Fonts/Consolas", Color.Blue, Color.Red);
            UpdateSoundStatus(null, null);
            m_ToggleSound.ToggleUp += onToggleSounds;
            m_ToggleSound.Scale = Vector2.One * 2f;
            m_ToggleSound.ToggleDown += onToggleSounds;
            m_SettingsManager.MutedChange += UpdateSoundStatus;
            ChooseableMenuItem Done = new ChooseableMenuItem(Game, "Done", @"Fonts/Consolas", Color.Blue, Color.Red);
            Done.Choose += onChooseDone;
            Done.Scale = Vector2.One * 2f;
            Add(MusicOption);
            Add(SoundFXOption);
            Add(m_ToggleSound);
            Add(Done);
            base.Initialize();
        }

        private void UpdateSoundStatus(object sender, EventArgs e)
        {
            m_ToggleSound.ExtraText = m_SettingsManager.SoundsMuted ? "Off" : "On";
        }

        private void onChooseDone(object i_Sender, EventArgs i_EventArgs)
        {
            this.ExitScreen();
        }

        private void onToggleSounds(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem toggledOption = i_Sender as SettingMenuItem;
            m_SettingsManager.ToggleSounds();
            UpdateSoundStatus(null, null);
        }

        protected override void OnActivated()
        {
            UpdateSoundStatus(null, null);
            base.OnActivated();
        }

        private void onSFXVolumeUp(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem volumeOption = i_Sender as SettingMenuItem;
            m_SettingsManager.SoundFXVolumeUp(0.1f);
            volumeOption.ExtraText = Math.Round(m_SettingsManager.SoundFXVolume * 100).ToString();
        }

        private void onSFXVolumeDown(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem volumeOption = i_Sender as SettingMenuItem;
            m_SettingsManager.SoundFXVolumeDown(0.1f);
            volumeOption.ExtraText = Math.Round(m_SettingsManager.SoundFXVolume * 100).ToString();
        }

        private void onBGMusicVolumeUp(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem volumeOption = i_Sender as SettingMenuItem;
            m_SettingsManager.BGMusicVolumeUp(0.1f);
            volumeOption.ExtraText = Math.Round(m_SettingsManager.BGMusicVolume * 100).ToString();
        }

        private void onBGMusicVolumeDown(object i_Sender, EventArgs i_EventArgs)
        {
            SettingMenuItem volumeOption = i_Sender as SettingMenuItem;
            m_SettingsManager.BGMusicVolumeDown(0.1f);
            volumeOption.ExtraText = Math.Round(m_SettingsManager.BGMusicVolume * 100).ToString();
        }
    }
}