namespace ButterflyCS.Monitor.Command
{
    /// <summary>
    /// The control command interpreter.
    /// </summary>
    public static class Control
    {
        private static readonly string[] subcommands = new string[] { "start", "stop", "reset", "status", "step", "speed" }; // The subcommands we can use

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
        public static string ParseArgs(string argString)
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
        private static string Start()
        {
            Machine.Run();
            return "Machine started";
        }

        /// <summary>
        /// Stops the machine.
        /// </summary>
        /// <returns>A message.</returns>
        private static string Stop()
        {
            Machine.Stop();
            return "Machine stopped";
        }

        /// <summary>
        /// Resets the machine.
        /// </summary>
        /// <returns>A message.</returns>
        private static string Reset()
        {
            Machine.Reset();
            return "Machine reset\n" + Status();
        }

        /// <summary>
        /// Prints the status of the CPU.
        /// </summary>
        /// <returns>A message.</returns>
        private static string Status()
        {
            string status = "Status:\n";

            // Add the registers
            status += "Registers:\n";
            status += "[A: " + Machine.PeekRegister("A") + "]";
            status += " [X: " + Machine.PeekRegister("X") + "]";
            status += " [Y: " + Machine.PeekRegister("Y") + "]";
            status += " [PC: " + Machine.PeekPC() + "]";
            status += " [SP: " + Machine.PeekRegister("SP") + "]";
            status += " [SR: " + Machine.PeekRegister("SR") + "]\n";

            return status;
        }

        /// <summary>
        /// Steps the machine.
        /// </summary>
        /// <returns>A message.</returns>
        private static string Step()
        {
            if (!Machine.isSingleStepping)
            {
                Machine.isSingleStepping = true;
            }

            Machine.Step();

            return "Machine step";
        }

        /// <summary>
        /// Sets the CPU speed.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A string.</returns>
        private static string Speed(string[] args)
        {
            // Strip the subcommand
            args = args[1..];

            // "args" should be 1 element long
            if (args.Length != 1)
            {
                if (args.Length == 0)
                {
                    // Return the current speed
                    return "Speed is " + Machine.CycleSpeed.ToString() + " Hz";
                }

                return RunHelpCommand();
            }

            // Grab the argument
            double speed = double.Parse(args[0]);

            // Set the speed
            Machine.CycleSpeed = speed;
            Machine.UpdateTimer();

            return "Speed set to " + args[0] + " Hz";
        }
    }
}
