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
        public static Task Main(string[] args)
        {
            // Initialize the machine
            Machine machine = new();
            machine.LoadDemoProgram();
            machine.Reset();
            machine.Start();
            return Task.CompletedTask;
        }
    }
}