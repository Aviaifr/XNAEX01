using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace GameInfrastructure.ServiceInterfaces
{
    public interface ISoundEffectsPlayer
    {
        void PlaySoundEffect(SoundEffect i_EffectToPlay);
    }
}
