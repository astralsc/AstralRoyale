using System;
using System.Threading;
using ClashRoyale.Utilities.Utils;

namespace ClashRoyale
{
    public static class Program
    {
        private static void Main()
        {
            Console.Title = "AstralRoyale: v1.0 (by: @fdz6 on GitHub)";

            Console.WriteLine(
            "    _        _             _ ____                   _      \n" +
            "   / \\   ___| |_ _ __ __ _| |  _ \\ ___  _   _  __ _| | ___ \n" +
            "  / _ \\ / __| __| '__/ _` | | |_) / _ \\| | | |/ _` | |/ _ \\\n" +
            " / ___ \\\\__ \\ |_| | | (_| | |  _ < (_) | |_| | (_| | |  __/\n" +
            "/_/   \\_\\___/\\__|_|  \\__,_|_|_| \\_\\___/ \\__, |\\__,_|_|\\___|\n" +
            "                                        |___/              ");

            Console.WriteLine("RetroRoyale fork by @fdz6");

            Resources.Initialize();

            if (ServerUtils.IsLinux())
            {
                Thread.Sleep(Timeout.Infinite);
            }
            else
            {
                Logger.Log("Press any key to shutdown the server.", null);
                Console.Read();
            }

            Shutdown();
        }

        public static async void Shutdown()
        {
            Console.WriteLine("Shutting down...");

            await Resources.Netty.Shutdown();

            try
            {
                Console.WriteLine("Saving players...");

                lock (Resources.Players.SyncObject)
                {
                    foreach (var player in Resources.Players.Values) player.Save();
                }

                Console.WriteLine("All players saved.");
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't save all players.");
            }

            await Resources.Netty.ShutdownWorkers();
        }

        public static void Exit()
        {
            Environment.Exit(0);
        }
    }
}