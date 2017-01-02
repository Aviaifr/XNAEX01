using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.ServiceInterfaces;
using GameInfrastructure.Managers;

namespace GameInfrastructure.ObjectModel
{
    public enum eActions
    {
        Left,Right,Shoot
    }

    public class PlayerInfo
    {
        private Dictionary<eActions, Keys> m_KeyBoardDictionary =
            new Dictionary<eActions, Keys>();

        private Dictionary<eActions, eInputButtons> m_MouseGamePadDictionary =
            new Dictionary<eActions, eInputButtons>();

        private bool m_isMouseControlled;

        public bool IsMouseControlled
        {
            get { return m_isMouseControlled; }
        }
        public ActionKeys GetKeys(eActions i_Action)
        {
            ActionKeys ActionKeys = new ActionKeys();
            if(thereIsKeyboardMapping(i_Action))
            {
                ActionKeys.KeyboardKey = m_KeyBoardDictionary[i_Action];
            }
            if(thereIsMouseKeypadMapping(i_Action))
            {
                ActionKeys.MouseGamepadKeys = m_MouseGamePadDictionary[i_Action];
            }

            return ActionKeys;
        }

        private bool thereIsMapping(eActions i_Action)
        {
            return (m_KeyBoardDictionary.ContainsKey(i_Action) 
                || m_MouseGamePadDictionary.ContainsKey(i_Action));
        }

        private bool thereIsKeyboardMapping(eActions i_Action)
        {
            return (m_KeyBoardDictionary.ContainsKey(i_Action));
        }

        private bool thereIsMouseKeypadMapping(eActions i_Action)
        {
            return (m_KeyBoardDictionary.ContainsKey(i_Action));
        }
    }
}
