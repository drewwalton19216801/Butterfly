using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButterflyCS.Monitor.Command
{
    /// <summary>
    /// The control command interpreter.
    /// </summary>
    public class Control
    {
        private readonly Machine _machine; // The machine to control
        private readonly string[] subcommands = new string[] { "start", "stop", "reset", "status", "step" }; // The subcommands we can use

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="machine">The machine.</param>
        public Control(Machine machine)
        {
            _machine = machine;

            // Check for null
            if (_machine == null)
            {
                throw new ArgumentNullException(nameof(machine));
            }
        }

        /// <summary>
        /// Displays the help message.
        /// </summary>
        /// <returns>A message</returns>
        public static string RunHelpCommand()
        {
            return "control - Control the CPU\n" +
                "Usage: control <subcommand>\n" +
                "Subcommands:\n" +
                "  start - Start the CPU\n" +
                "  stop - Stop the CPU\n" +
                "  reset - Reset the CPU\n" +
                "  status - Get the status of the CPU\n" +
                "  step - Step the CPU";
        }

        /// <summary>
        /// Parses the command arguments.
        /// </summary>
        /// <param name="argString">The arg string.</param>
        /// <returns>The message</returns>
        public string ParseArgs(string argString)
        {
            // Split the arguments
            string[] args = argString.Split(' ');

            // Run the command
            return args[0] switch
            {
                "start" => Start(),
                "stop" => Stop(),
                "reset" => Reset(),
                "status" => Status(),
                "step" => Step(),
                _ => RunHelpCommand(),
            };
        }

        /// <summary>
        /// Starts the machine.
        /// </summary>
        /// <returns>A message.</returns>
        private string Start()
        {
            _machine.Run();
            return "Machine started";
        }

        /// <summary>
        /// Stops the machine.
        /// </summary>
        /// <returns>A message.</returns>
        private string Stop()
        {
            _machine.Stop();
            return "Machine stopped";
        }

        /// <summary>
        /// Resets the machine.
        /// </summary>
        /// <returns>A message.</returns>
        private string Reset()
        {
            _machine.Reset();
            return "Machine reset";
        }

        /// <summary>
        /// Prints the status of the CPU.
        /// </summary>
        /// <returns>A message.</returns>
        private string Status()
        {
            // TODO: Add status
            return "Status";
        }

        /// <summary>
        /// Steps the machine.
        /// </summary>
        /// <returns>A message.</returns>
        private string Step()
        {
            _machine.Step();
            return "Machine step";
        }
    }
}
