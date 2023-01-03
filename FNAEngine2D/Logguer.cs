using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Logguer
    /// </summary>
    public static class Logguer
    {
        /// <summary>
        /// GameTimer
        /// </summary>
        private static Stopwatch _gameTimer;

        /// <summary>
        /// Tick on the last log
        /// </summary>
        private static long _lastLogTick = 0;

        /// <summary>
        /// Prefix pour les logs
        /// </summary>
        private static readonly string _prefix = Process.GetCurrentProcess().ProcessName;


        /// <summary>
        /// Global timer
        /// </summary>
        static Logguer()
        {
            _gameTimer = Stopwatch.StartNew();
        }


        /// <summary>
        /// Log an info
        /// </summary>
        public static void Info(string info)
        {
            Console.WriteLine(GetPrefixLog() + " " + info);
        }

        /// <summary>
        /// Log an error
        /// </summary>
        public static void Error(string detail, Exception ex)
        {
            Console.WriteLine(GetPrefixLog() + " " + detail + " - " + ex.ToString());
        }

        /// <summary>
        /// Log an error
        /// </summary>
        public static void Error(Exception ex)
        {
            Console.WriteLine(GetPrefixLog() + " ERROR: " + ex.ToString());
        }

        /// <summary>
        /// Get the prefix for the log
        /// </summary>
        private static string GetPrefixLog()
        {
            string prefix = _prefix + " " + ((decimal)_gameTimer.ElapsedTicks / Stopwatch.Frequency).ToString("0.0000") + " +" + ((decimal)(_gameTimer.ElapsedTicks - _lastLogTick) / Stopwatch.Frequency).ToString("0.0000");
            _lastLogTick = _gameTimer.ElapsedTicks;
            return prefix;
        }

    }
}
