namespace ButterflyCS.Monitor.Command
{
    /// <summary>
    /// The monitor command interpreter
    /// </summary>
    public class Interpreter
    {
        private readonly Machine _machine; // The machine that this interpreter is attached to
        private readonly Mem _memInterpreter; // The memory interpreter
        private readonly Control _controlInterpreter; // The control interpreter
        private readonly Reg _regInterpreter; // The register interpreter
        private bool _commandRunning = false; // Whether or not a command is running

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpreter"/> class.
        /// </summary>
        /// <param name="machine">The machine.</param>
        public Interpreter(Machine machine)
        {
            _machine = machine;

            // Initialize the interpreters
            _memInterpreter = new(_machine);
            _controlInterpreter = new(_machine);
            _regInterpreter = new(_machine);
        }

        /// <summary>
        /// Interprets the command.
        /// </summary>
        /// <param name="cmdArgs">The command + args.</param>
        /// <returns>A string.</returns>
        public string? InterpretCommand(string cmdArgs)
        {
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
                    result = _memInterpreter.ParseArgs(cmdArgs);
                    _commandRunning = false;
                }
                else if (cmd == "control")
                {
                    _commandRunning = true;
                    result = _controlInterpreter.ParseArgs(cmdArgs);
                    _commandRunning = false;
                } else if (cmd == "reg")
                {
                    _commandRunning = true;
                    result = _regInterpreter.ParseArgs(cmdArgs);
                    _commandRunning = false;
                }

                if (cmd == "quit")
                {
                    // Quit the program
                    Environment.Exit(0);
                }
            } 
            else
            {
                result = "A command is already running.";
            }

            return result;
        }
    }
}
