using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp6502
{
    /// <summary>
    /// Logging subsystem.
    /// </summary>
    internal static class Log
    {
        /// <summary>
        /// An informational message.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="message">The message.</param>
        public static void Info(string subsystem, string message)
        {
            Console.WriteLine($"[INFO] {subsystem}: {message}");
        }

        /// <summary>
        /// A warning message.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="message">The message.</param>
        public static void Warning(string subsystem, string message)
        {
            Console.WriteLine($"[WARNING] {subsystem}: {message}");
        }

        /// <summary>
        /// An error message.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="message">The message.</param>
        public static void Error(string subsystem, string message)
        {
            Console.WriteLine($"[ERROR] {subsystem}: {message}");
        }
    }
}
