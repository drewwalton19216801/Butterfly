using Sharp6502;

namespace ButterflyCS.Monitor.Command
{
    /// <summary>
    /// The reg command interpreter.
    /// </summary>
    public class Reg
    {
        private readonly Machine _machine; // The machine to control
        private readonly string[] subcommands = new string[] { "read", "write" }; // The subcommands we can use

        /// <summary>
        /// The register enum.
        /// </summary>
        /// <remarks>Only used here.</remarks>
        private enum Register
        {
            A,
            X,
            Y,
            PC,
            SP,
            P
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reg"/> class.
        /// </summary>
        /// <param name="machine">The machine.</param>
        public Reg(Machine machine)
        {
            _machine = machine;

            // Check for null
            if (_machine == null)
            {
                throw new ArgumentNullException(nameof(machine));
            }
        }

        /// <summary>
        /// Parses the arguments.
        /// </summary>
        /// <param name="argString">The arg string.</param>
        /// <returns>A message.</returns>
        public string ParseArgs(string argString)
        {
            // Split the arguments into an array
            string[] args = argString.Split(' ');

            // Check if the subcommand is valid
            if (!subcommands.Contains(args[0]))
            {
                return RunHelpCommand();
            }

            // Run the subcommand
            return args[0] switch
            {
                "read" => Read(args),
                "write" => Write(args),
                _ => RunHelpCommand(),
            };
        }

        /// <summary>
        /// Runs the help command.
        /// </summary>
        /// <returns>A string.</returns>
        public static string RunHelpCommand()
        {
            return "reg - Read/write registers\n" +
                "Usage: reg <subcommand>\n" +
                "Subcommands:\n" +
                "  read - Read a register\n" +
                "  write - Write a register\n";
        }

        /// <summary>
        /// Reads a register.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>A message.</returns>
        private string Read(string[] args)
        {
            // Strip the subcommand
            args = args[1..];

            // Check if the register is valid
            if (!Enum.TryParse(args[0], out Register reg))
            {
                return "Invalid register!";
            }

            if (reg != Register.PC)
            {
                return _machine.PeekRegister(reg.ToString());
            } else
            {
                return _machine.PeekPC();
            }
        }

        /// <summary>
        /// Writes a register.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A message.</returns>
        private string Write(string[] args)
        {
            // Strip the subcommand
            args = args[1..];

            // Check if the register is valid
            if (!Enum.TryParse(args[0], out Register reg))
            {
                return "Invalid register!";
            }


            if (reg != Register.PC)
            {
                // Convert the data to a byte
                if (!byte.TryParse(args[1], out byte data))
                {
                    return "Invalid data!";
                }
                _machine.PokeRegister(reg.ToString(), data);
                return _machine.PeekRegister(reg.ToString());
            } else
            {
                // Convert the data to a ushort
                if (!ushort.TryParse(args[1], out ushort data))
                {
                    return "Invalid data!";
                }
                _machine.PokePC(data);
                return _machine.PeekPC();
            }
        }
    }
}
