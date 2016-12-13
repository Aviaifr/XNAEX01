﻿using System;

namespace Space_Invaders
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new SpaceInvaderGame())
            {
                game.Run();
            }
        }
    }
#endif
}
