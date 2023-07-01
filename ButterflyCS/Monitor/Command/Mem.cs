using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButterflyCS.Monitor.Command
{
    /// <summary>
    /// The mem command interpreter.
    /// </summary>
    public class Mem
    {
        private readonly Machine _machine;
        private readonly string[] subcommands = new string[] { "read", "write" };

        /// <summary>
        /// Initializes a new instance of the <see cref="Mem"/> class.
        /// </summary>
        /// <param name="machine">The machine.</param>
        public Mem(Machine machine)
        {
            _machine = machine;

            // Check for null
            if (_machine == null)
            {
                throw new ArgumentNullException(nameof(machine));
            }
        }

        /// <summary>
        /// Runs the help command.
        /// </summary>
        /// <returns>The text to be displayed.</returns>
        public static string RunHelpCommand()
        {
            return "mem - Read or write memory.\n" +
                "mem read <address> - Read a byte from memory.\n" +
                "mem write <address> <data> - Write a byte to memory.";
        }

        /// <summary>
        /// Parses the command arguments.
        /// </summary>
        /// <param name="cmdString">The arguments.</param>
        /// <returns>A string to be displayed.</returns>
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
                "read" => ReadCommand(args),
                "write" => WriteCommand(args),
                _ => "Invalid subcommand.",
            };
        }

        /// <summary>
        /// Reads a byte from memory.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A string.</returns>
        private string ReadCommand(string[] args)
        {
            // Strip the subcommand from the args
            args = args[1..];

            // "args" should be 1 element long (the address)
            if (args.Length != 1)
            {
                return "Invalid number of arguments.";
            }

            // Convert the address to a ushort
            ushort address;
            try
            {
                address = Convert.ToUInt16(args[0], 16);
            }
            catch (Exception)
            {
                return "Invalid address.";
            }

            // Read the byte from memory
            string data = _machine.PeekMemory(address);

            // Return the data
            return data;
        }

        /// <summary>
        /// Writes the command.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A string.</returns>
        private string WriteCommand(string[] args)
        {
            // Strip the subcommand from the args
            args = args[1..];

            // "args" should be 2 elements long (the address, and the data)
            if (args.Length != 2)
            {
                return "Invalid number of arguments.";
            }
            ushort address;
            try
            {
                address = Convert.ToUInt16(args[0], 16);
            }
            catch (Exception)
            {
                return "Invalid address.";
            }
            byte data;
            try
            {
                data = Convert.ToByte(args[1], 16);
            }
            catch (Exception)
            {
                return "Invalid data.";
            }
            _machine.PokeMemory(address, data);

            // Return a message, converting the data to 0x00 format, and the address to 0x0000 format
            return $"Wrote 0x{data:X2} to 0x{address:X4}.";
        }
    }
}
