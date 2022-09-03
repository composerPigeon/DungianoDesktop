using System;

namespace DungianoDesktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new DungianoGame())
                game.Run();
        }
    }
}
