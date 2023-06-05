namespace Sharp6502
{
    /// <summary>
    /// Logging subsystem.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// The log message type.
        /// </summary>
        private enum LogType
        {
            Info,
            Warning,
            Error,
            Debug
        }

        /// <summary>
        /// An informational message.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="message">The message.</param>
        public static void Info(string subsystem, string message)
        {
            LogMessage(LogType.Info, subsystem, message);
        }

        /// <summary>
        /// A warning message.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="message">The message.</param>
        public static void Warning(string subsystem, string message)
        {
            LogMessage(LogType.Warning, subsystem, message);
        }

        /// <summary>
        /// An error message.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="message">The message.</param>
        public static void Error(string subsystem, string message)
        {
            LogMessage(LogType.Error, subsystem, message);
        }

        /// <summary>
        /// A debug message.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="message">The message.</param>
        public static void Debug(string subsystem, string message)
        {
            LogMessage(LogType.Debug, subsystem, message);
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="message">The message.</param>
        private static void LogMessage(LogType type, string subsystem, string message)
        {
            ConsoleColor currentColor = Console.ForegroundColor;

            switch (type)
            {
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[INFO] ");
                    break;
                case LogType.Debug:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("[DEBUG] ");
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[ERROR] ");
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[WARNING] ");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(subsystem);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(": ");
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }
    }
}
