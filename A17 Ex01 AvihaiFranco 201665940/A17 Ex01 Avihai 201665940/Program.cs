using System;

namespace A17_Ex01_Avihai_201665940
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
