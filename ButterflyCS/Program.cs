using Sharp6502;

namespace ButterflyCS
{
    /// <summary>
    /// The program.
    /// </summary>
    public static class Program
    {
        public static readonly string subsystem = "ButterflyCS";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>A Task.</returns>
        public static Task Main(string[] args)
        {
            // Get the ROM file path from the command-line arguments, if
            // one was provided
            string? romFilePath = args.Length > 0 ? args[0] : null;

            // If we got a null ROM file path, then we'll use the default
            // ROM file path, but tell the user that they can specify a
            // ROM file path via the command-line arguments
            if (romFilePath == null)
            {
                romFilePath = Path.Combine(
                    Environment.CurrentDirectory,
                    "rom.bin"
                );

                Log.Info(
                    subsystem,
                    "No ROM file path was specified via the command-line arguments. Using default ROM file path: "+romFilePath
                );
            }

            // Initialize the logging subsystem
            Log.DebugEnabled = true;

            // Initialize the machine
            Machine machine = new();

            // Load the ROM
            machine.LoadProgram(romFilePath, 0x8000);

            // Set the CPU variant
            machine.cpu.cpuVariant = CPU.Variant.CMOS_65C02;

            // Initialize the GUI
            MainWin mainWin = new(machine);
            mainWin.StartApplication();
            return Task.CompletedTask;
        }
    }
}