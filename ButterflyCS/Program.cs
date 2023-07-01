using Sharp6502;

namespace ButterflyCS
{
    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        public static readonly string subsystem = "ButterflyCS";

        private static readonly Machine machine = new();

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
            MachineStart();

            // Create the threads for the UIs
            Thread terminalThread = new(RunTerminalUI);
            Thread guiThread = new(RunGUI);

            // Start the threads
            terminalThread.Start();
            guiThread.Start();

            // Wait for both threads to finish
            terminalThread.Join();
            guiThread.Join();

            return Task.CompletedTask;
        }

        /// <summary>
        /// The terminal UI task.
        /// </summary>
        /// <returns>A Task.</returns>
        public static void RunTerminalUI()
        {
            Log.Info(subsystem, "Terminal UI started.");
        }

        /// <summary>
        /// The main UI task.
        /// </summary>
        /// <returns>A Task.</returns>
        public static void RunGUI()
        {
            Log.Info(subsystem, "Main UI started.");

            // Initialize the GUI
            MainWin mainWin = new(machine);
            mainWin.StartApplication();
        }

        /// <summary>
        /// The task that sets up the machine.
        /// </summary>
        /// <returns>A Task.</returns>
        public static void MachineStart()
        {
            Log.Info(subsystem, "Machine initializing");

            // Initialize the logging subsystem
            Log.DebugEnabled = true;

            // Load the ROM
            machine.LoadProgram(romFilePath, 0x8000);

            // Set the CPU variant
            machine.cpu.cpuVariant = CPU.Variant.CMOS_65C02;

            // Set the CPU speed to 1 Hz
            machine.CycleSpeed = 1;

            Log.Info(subsystem, "Machine initialized");
        }
    }
}