using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace GameInfrastructure.Managers
{
    public class PlayersManager : GameService, IPlayersManager
    {
        private Game m_Game;
        private IInputManager m_InputManager;
        private Dictionary<String, PlayerInfo> m_PlayersInfo = 
            new Dictionary<string, PlayerInfo>();

        public PlayersManager(Game i_Game) :
            base(i_Game, int.MaxValue)
        {
            m_Game = i_Game;
        }

        public override void Initialize()
            {
            m_InputManager = m_Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            }

        protected override void RegisterAsService()
            {
            this.Game.Services.AddService(typeof(IPlayersManager), this);
            }

        //public IInputManager InputManager
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        public Dictionary<string, PlayerInfo> PlyersInfo
        {
            get
            {
                return m_PlayersInfo;
            }

            set
            {
                m_PlayersInfo = value;
            }
        }

        public bool DidPress(String i_PlayerId, eActions i_Action)
        {
            bool keyboardPress = false;
            bool mouseGamepadPress = false;
            ActionKeys actionKeys = m_PlayersInfo[i_PlayerId].GetKeys(i_Action);
            if(actionKeys.KeyboardKey != null)
            {
                if (i_Action == eActions.Shoot)
                {
                    keyboardPress = m_InputManager.KeyPressed(actionKeys.KeyboardKey.Value);
                }
                else
                {
                    keyboardPress = m_InputManager.KeyHeld(actionKeys.KeyboardKey.Value);
                }
            }
            if(actionKeys.MouseGamepadKeys != null)
        {
                mouseGamepadPress = m_InputManager.ButtonPressed(actionKeys.MouseGamepadKeys.Value);
            }

            return keyboardPress || mouseGamepadPress;
        }
    }
}
