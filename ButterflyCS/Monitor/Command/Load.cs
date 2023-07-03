using Microsoft.Toolkit.HighPerformance.Buffers;

namespace ButterflyCS.Monitor.Command
{
    /// <summary>
    /// The load command
    /// </summary>
    public static class Load
    {
        private static readonly string[] subcommands = new string[] { "file" }; // The supported subcommands

        /// <summary>
        /// Parse the arguments for the load command
        /// </summary>
        /// <param name="argString"></param>
        /// <returns>A message.</returns>
        public static string ParseArgs(string argString)
        {
            // Split the arguments into an array
            string[] args = argString.Split(' ');

            // Check if the subcommand is valid
            if (!subcommands.Contains(args[0]))
            {
                return "Error: Invalid subcommand\n" + Help();
            }

            // Run the subcommand
            return args[0] switch
            {
                "file" => File(args),
                _ => "Error: Invalid subcommand\n" + Help(),
            };
        }

        private static string Help()
        {
            return "\nload - Load a file into the emulator\n" +
                "Usage: load <subcommand> <path> <address>\n" +
                "Subcommands:\n" +
                "    file <path> <address> - Load a rom file into the emulator at the specified address\n" +
                "                              : Only supports \"*.bin\" (vasm binary output) files\n";
        }

        private static string File(string[] args)
        {
            // Strip the subcommand
            args = args[1..];

            // "args" should be 2 elements long
            if (args.Length != 2)
            {
                return "Error: Invalid number of arguments\n" + Help();
            }

            string path = args[0];
            string loadAddress = args[1];

            // Strip the quotes from the path, if any
            if (path.StartsWith('"') && path.EndsWith('"'))
            {
                path = path[1..^1];
            }

            // Strip single quotes too
            if (path.StartsWith('\'') && path.EndsWith('\''))
            {
                path = path[1..^1];
            }

            // Check if the file exists
            if (!System.IO.File.Exists(path))
            {
                return "Error: File does not exist\n";
            }

            // Check if the address is valid and in range (0x0000 - 0xFFFF)
            if (!ushort.TryParse(loadAddress, System.Globalization.NumberStyles.HexNumber, null, out ushort address))
            {
                return "Error: Invalid address\n";
            }

            Machine.LoadProgram(path, address);

            // Get the file name from the path
            string fileName = System.IO.Path.GetFileName(path);

            string loadedString = "Loaded file \"" + fileName + "\" at address 0x" + address.ToString("X4") + "\n";

            Machine.Reset();

            return loadedString + "CPU has been reset\n";
        }
    }
}
