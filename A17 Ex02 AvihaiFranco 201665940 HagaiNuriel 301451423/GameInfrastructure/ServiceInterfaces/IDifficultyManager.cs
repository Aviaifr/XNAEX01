using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ObjectModel.Screens;

namespace GameInfrastructure.ServiceInterfaces
{
    public interface IDifficultyManager
    {
        GameScreen GameToMonitor { get; set; }

        void IncreaseDifficulty();
        void ResetDifficulty();
    }
}
