using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInfrastructure.ObjectModel;
using Microsoft.Xna.Framework.Input;

namespace GameInfrastructure.ServiceInterfaces
{
    public struct ActionKeys
    {
        private eInputButtons? m_MouseGamepadKeys;
        private Keys? m_KeyboardKey;

        public eInputButtons? MouseGamepadKeys
        {
            get { return m_MouseGamepadKeys; }
            set { m_MouseGamepadKeys = value; }
        }

        public Keys? KeyboardKey
        {
            get { return m_KeyboardKey; }
            set { m_KeyboardKey = value; }
        }
    }

    public interface IPlayersManager
    {
        //IInputManager InputManager { get; set; }
        Dictionary<String,PlayerInfo> PlyersInfo { get; set; }
        bool DidPress(String i_PlayerId, eActions i_Action);
    }
}
