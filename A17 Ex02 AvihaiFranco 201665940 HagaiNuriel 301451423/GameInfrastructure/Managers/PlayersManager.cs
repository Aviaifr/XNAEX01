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
        private Dictionary<string, PlayerInfo> m_PlayersInfo = 
            new Dictionary<string, PlayerInfo>();
        private List<IPlayer> m_Players = new List<IPlayer>();

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

        public IPlayer GetPlayerByIndex(int i_Index)
        {
            IPlayer result = null;
            if (i_Index < m_Players.Count)
            {
                result = m_Players[i_Index];
            }

            return result;
        }

        public void ClearPlayers()
        {
            m_Players = new List<IPlayer>();
        }

        public void AddPlayer(IPlayer i_PlayerToAdd)
        {
            m_Players.Add(i_PlayerToAdd);
        }

        public bool DidPress(string i_PlayerId, eActions i_Action)
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
