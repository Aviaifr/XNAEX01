using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInfrastructure.ServiceInterfaces
{
    public interface ISettingsManager
    {
        event EventHandler MutedChange;

        event EventHandler MouseVisibilityChange;
        
        event EventHandler ResizeableChange;
        
        event EventHandler FullScreenChange;

        int Level { get; set; }
        
        bool SoundsMuted { get; set; }

        float BGMusicVolume { get; set; }
        
        float SoundFXVolume { get; set; }
        
        int NumOfPlayers { get; set; }
        
        bool IsMouseVisible { get; set; }
        
        bool IsResizeable { get; set; }
        
        bool IsFullScreen { get; set; }
        
        void BGMusicVolumeUp(float i_VolumeToAdd);

        void BGMusicVolumeDown(float i_VolumeToDecrease);
        
        void SoundFXVolumeUp(float i_VolumeToAdd);
        
        void SoundFXVolumeDown(float i_VolumeToDecrease);
        
        void ToggleSounds();
        
        void ToggleMouseVisibility();
        
        void ToggleWindowResizeable();
        
        void ToggleFullScreen();
    }
}
