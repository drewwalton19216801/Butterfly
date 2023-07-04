using System.Runtime.CompilerServices;
using Terminal.Gui;

namespace ButterflyCS.Monitor.Command
{
    /// <summary>
    /// The monitor command interpreter
    /// </summary>
    public static class Interpreter
    {
        private static bool _commandRunning = false; // Whether or not a command is running

        private static string[] supportedCommands = new string[] { "mem", "control", "reg", "load", "help", "quit" }; // The supported commands

        /// <summary>
        /// Displays the usage.
        /// </summary>
        /// <returns>A message.</returns>
        private static string DisplayUsage()
        {
            return "ButterflyCS Monitor\n" +
                "Usage: <command> <args>\n" +
                "Commands:\n" +
                "  mem - Read/write memory\n" +
                "  control - Control the machine\n" +
                "  reg - Read/write registers\n" +
                "  load - Load a file into memory\n" +
                "  help - Display this help message\n" +
                "  quit - Quit the program\n";
        }

        /// <summary>
        /// Interprets the command.
        /// </summary>
        /// <param name="cmdArgs">The command + args.</param>
        /// <returns>A boolean value that tells the Monitor if it needs
        /// to update the UI</returns>
        public static bool InterpretCommand(string cmdArgs)
        {
            bool needsReset = false;

            // Split the command into an array
            string[] args = cmdArgs.Split(' ');

            // Grab the first argument as the command
            string cmd = args[0];

            string result = string.Empty;

            // Remove the command from the array
            args = args[1..];

            // Convert the array to a string
            cmdArgs = string.Join(' ', args);

            // Run the command
            if (!_commandRunning)
            {
                if (cmd == "mem")
                {
                    _commandRunning = true;
                    result = Mem.ParseArgs(cmdArgs);
                    _commandRunning = false;
                }
                else if (cmd == "control")
                {
                    _commandRunning = true;
                    result = Control.ParseArgs(cmdArgs);
                    needsReset = true;
                    _commandRunning = false;
                } else if (cmd == "reg")
                {
                    _commandRunning = true;
                    result = Reg.ParseArgs(cmdArgs);
                    needsReset = true;
                    _commandRunning = false;
                }
                else if (cmd == "load")
                {
                    _commandRunning = true;
                    result = Load.ParseArgs(cmdArgs);
                    needsReset = true;
                    _commandRunning = false;
                }
                else if (cmd == "help")
                {
                    result = DisplayUsage();
                }
                else if (cmd == "quit")
                {
                    if (MessageBox.Query(50, 7, "Quit", "Are you sure you want to quit?", "Yes", "No") == 0)
                    {
                        // Quit everything and die
                        Environment.Exit(0);
                    }
                } else
                {
                    result = "Invalid command.\n\n" + DisplayUsage();
                }
            } 
            else
            {
                result = "A command is already running.";
            }

            MonitorOutput.Add(result);
            return needsReset;
        }
    }
}
