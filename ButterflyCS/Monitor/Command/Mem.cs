namespace ButterflyCS.Monitor.Command
{
    /// <summary>
    /// The mem command interpreter.
    /// </summary>
    public static class Mem
    {
        private static readonly string[] subcommands = new string[] { "read", "write" };

        /// <summary>
        /// Runs the help command.
        /// </summary>
        /// <returns>The text to be displayed.</returns>
        public static string RunHelpCommand()
        {
            return "\nmem - Read/write memory\n" +
                "Usage: mem <subcommand>\n" +
                "Subcommands:\n" +
                "  read <address> - Read a byte from memory\n" +
                "  write <address> <data> - Write a byte to memory\n";
        }

        /// <summary>
        /// Parses the command arguments.
        /// </summary>
        /// <param name="argString">The arguments.</param>
        /// <returns>A string to be displayed.</returns>
        public static string ParseArgs(string argString)
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
                "read" => "Data: " + Read(args),
                "write" => Write(args),
                _ => "Invalid subcommand.",
            };
        }

        /// <summary>
        /// Reads a byte from memory.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A string.</returns>
        private static string Read(string[] args)
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
            string data = Machine.PeekMemory(address);

            // Return the data
            return data;
        }

        /// <summary>
        /// Writes the command.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A string.</returns>
        private static string Write(string[] args)
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
            Machine.PokeMemory(address, data);

            // Return a message, converting the data to 0x00 format, and the address to 0x0000 format
            return $"Wrote 0x{data:X2} to 0x{address:X4}.";
        }
    }
}
