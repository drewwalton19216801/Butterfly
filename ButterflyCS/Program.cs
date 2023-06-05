using Raylib_CsLo;
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
        public static Task Main(string[] args)
        {
            int cyclesToExecute = 14;

            // Initialize the machine
            Machine machine = new();
            machine.LoadDemoProgram();
            machine.Reset();

            Raylib.InitWindow(800, 600, "ButterflyCS");
            Raylib.SetTargetFPS(60);
            // Main emulator loop
            while (!Raylib.WindowShouldClose())
            {
                if (cyclesToExecute > 0)
                {
                    // Execute the next instruction
                    machine.Cycle();
                    cyclesToExecute--;
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.GREEN);
                Raylib.DrawFPS(10, 10);
                Raylib.DrawText("Raylib_CsLo", 10, 30, 20, Raylib.WHITE);
                Raylib.EndDrawing();
            }

            // Log the state of the A register
            Log.Info(Machine.subsystem, $"A register: {machine.cpu.registers.A}");

            // Log the state of memory address 0x0200
            Log.Info(Machine.subsystem, $"Memory address 0x0200: {machine.cpu.memory.Read(0x200)}");

            Raylib.CloseWindow();
            return Task.CompletedTask;
        }
    }
}