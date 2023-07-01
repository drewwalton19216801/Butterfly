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
        private readonly string[] subcommands = new string[] { "start", "stop", "reset", "status", "step", "speed" }; // The subcommands we can use

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
            return "\ncontrol - Control the CPU\n" +
                "Usage: control <subcommand>\n" +
                "Subcommands:\n" +
                "  start - Start the CPU\n" +
                "  stop - Stop the CPU\n" +
                "  reset - Reset the CPU\n" +
                "  status - Get the status of the CPU\n" +
                "  step - Step the CPU\n" +
                "  speed - Set the CPU speed in Hz\n";
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
                "speed" => Speed(args),
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
            return "Machine reset\n" + Status();
        }

        /// <summary>
        /// Prints the status of the CPU.
        /// </summary>
        /// <returns>A message.</returns>
        private string Status()
        {
            string status = "Status:\n";

            // Add the registers
            status += "Registers:\n";
            status += "[A: " + _machine.PeekRegister("A") + "]";
            status += " [X: " + _machine.PeekRegister("X") + "]";
            status += " [Y: " + _machine.PeekRegister("Y") + "]";
            status += " [PC: " + _machine.PeekPC() + "]";
            status += " [SP: " + _machine.PeekRegister("SP") + "]";
            status += " [SR: " + _machine.PeekRegister("SR") + "]\n";

            return status;
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

        /// <summary>
        /// Sets the CPU speed.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A string.</returns>
        private string Speed(string[] args)
        {
            // Strip the subcommand
            args = args[1..];

            // "args" should be 1 element long
            if (args.Length != 1)
            {
                if (args.Length == 0)
                {
                    // Return the current speed
                    return "Speed is " + _machine.CycleSpeed.ToString() + " Hz";
                }

                return RunHelpCommand();
            }

            // Grab the argument
            double speed = double.Parse(args[0]);

            // Set the speed
            _machine.CycleSpeed = speed;
            _machine.UpdateTimer();

            return "Speed set to " + args[0] + " Hz";
        }
    }
}
