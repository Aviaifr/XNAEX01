using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameInfrastructure.ObjectModel
{
    public class GameComponentEventArgs<ComponentType> : EventArgs
            where ComponentType : IGameComponent
    {
        private ComponentType m_Component;

        public GameComponentEventArgs(ComponentType gameComponent)
        {
            m_Component = gameComponent;
        }

        public ComponentType GameComponent
        {
            get { return m_Component; }
        }
    }
}
