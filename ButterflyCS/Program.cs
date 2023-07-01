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
            machine.Init(romFilePath, CPU.Variant.NMOS_6502, 1);

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

            // Loop for 20 seconds, printing the number of seconds left every second
            for (int i = 20; i > 0; i--)
            {
                Console.WriteLine($"DEBUG DEBUG DEBUG {i} seconds left");
                Thread.Sleep(1000);
            }

            // Quit the application
            Environment.Exit(0);
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
    }
}