using System;

namespace Adastra
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Adastra game = new Adastra())
            {
                game.Run();
            }
        }
    }
#endif
}

