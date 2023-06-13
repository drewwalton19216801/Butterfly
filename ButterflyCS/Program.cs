using Sharp6502;

namespace ButterflyCS
{
    /// <summary>
    /// The program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>A Task.</returns>
        public static Task Main()
        {
            // Initialize the logging subsystem
            Log.DebugEnabled = true;

            // Initialize the machine
            Machine machine = new();
            machine.LoadDemoProgram();
            machine.Reset();

            // Initialize the GUI
            MainWin mainWin = new(machine);
            mainWin.StartApplication();
            return Task.CompletedTask;
        }
    }
}