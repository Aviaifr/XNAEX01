using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace GameInfrastructure.Managers
{
    public class PlayersManager : IPlayersManager
    {
        public IInputManager InputManager
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<string, PlayerInfo> PlyersInfo
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool DidPress(eActions i_Action)
        {
            throw new NotImplementedException();
        }
    }
}
