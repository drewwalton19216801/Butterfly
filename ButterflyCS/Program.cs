using Sharp6502;
using Terminal.Gui;

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
            // Clear the console
            Console.Clear();

            // Enable debug messages if the "--debug" flag is present
            if (args.Contains("--debug"))
            {
                Log.EnableDebugMessages();
            } else
            {
                Log.DisableDebugMessages();
            }

            machine.Init(romFilePath, CPU.Variant.NMOS_6502, 1);

            // Create the threads for the UIs
            Thread terminalThread = new(RunTerminalUI);
            Thread guiThread = new(RunGUI);

            // Start the threads
            terminalThread.Start();
            guiThread.Start();

            // Wait for the threads to finish
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
            Application.Init();
            var _machine = machine;
            // Run the terminal UI, passing the machine
            // PREV: Application.Run<Monitor.Monitor>();
            Application.Top.Add(new Monitor.Monitor(_machine));
            Application.Run();

            // Quit the application
            Application.Shutdown();
            Environment.Exit(0);
        }

        /// <summary>
        /// The main UI task.
        /// </summary>
        /// <returns>A Task.</returns>
        public static void RunGUI()
        {
            // Initialize the GUI
            GUI.MainWin mainWin = new(machine);
            mainWin.ShowWindow();

            // If the GUI is closed, quit the application
            Environment.Exit(0);
        }
    }
}