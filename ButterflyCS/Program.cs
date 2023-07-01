using Sharp6502;

namespace ButterflyCS
{
    /// <summary>
    /// The program.
    /// </summary>
    public static class Program
    {
        public static readonly string subsystem = "ButterflyCS";

        public static readonly string romFilePath = Path.Combine(
            Environment.CurrentDirectory,
            "rom.bin"
        );

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>A Task.</returns>
        public static Task Main(string[] args)
        {
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