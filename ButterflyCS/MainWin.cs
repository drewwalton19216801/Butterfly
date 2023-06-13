using System;
using System.Timers;
using Timer = System.Timers.Timer;
using Raylib_CsLo;
using Sharp6502;

namespace ButterflyCS
{
    /// <summary>
    /// The main GUI window.
    /// </summary>
    public class MainWin
    {
        /// <summary>
        /// The subsystem (for logging).
        /// </summary>
        private static readonly string subsystem = "GUI";

        /// <summary>
        /// The machine.
        /// </summary>
        private readonly Machine machine;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWin"/> class.
        /// </summary>
        /// <param name="_machine">The _machine.</param>
        public MainWin(Machine _machine)
        {
            Log.Debug(subsystem, "Initializing GUI");
            machine = _machine;
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        public void StartApplication()
        {
            // Initialize the window
            Raylib.InitWindow(800, 600, "Butterfly 6502 Emulator");

            // Set the framerate
            Raylib.SetTargetFPS(60);

            // Set up the machine
            machine.isRunning = false;
            machine.isPaused = true;
            machine.isSingleStepping = false;

            // Start the CPU timer
            machine.cpuTimer.Start();

            // Main loop
            while (!Raylib.WindowShouldClose())
            {
                // Begin GUI drawing
                Raylib.BeginDrawing();
                
                // Draw the machine's state
                DrawMachineStateScreen();

                // End GUI drawing
                Raylib.EndDrawing();

                // Check for pause (Ctrl+P)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_P))
                {
                    machine.isPaused = !machine.isPaused;
                    machine.isSingleStepping = false;
                }
                // Check for single-step (Ctrl+S)
                else if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                {
                    machine.isSingleStepping = true;
                    machine.isPaused = true;
                }

                // If machine is in single-step mode, execute one cycle if the user presses the spacebar
                if (machine.isSingleStepping && Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    Log.Debug(subsystem, "Single-stepping");
                    machine.cpu.Clock();
                }

                // Check for speed increase (Ctrl+PgUp)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_UP))
                {
                    // Increase the speed by 2 Hz
                    machine.IncreaseSpeed(2);
                }

                // Check for speed decrease (Ctrl+PgDn)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_DOWN))
                {
                    // Decrease the speed by 2 Hz
                    machine.DecreaseSpeed(2);
                }

                // Check for reset (Ctrl+R)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_R))
                {
                    // Reset the machine
                    machine.Reset();
                }

                // Check for quit (Ctrl+Q)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
                {
                    // Quit the application
                    Log.Info(subsystem, "Quitting application");
                    break;
                }
            }

            // Close the window
            Raylib.CloseWindow();
        }

        /// <summary>
        /// Draw the machine's state.
        /// </summary>
        private void DrawMachineStateScreen()
        {
            Raylib.ClearBackground(Raylib.GREEN);
            Raylib.DrawFPS(10, 10);

            // Draw the registers
            Raylib.DrawText("Registers", 10, 30, 20, Raylib.BLACK);
            Raylib.DrawText($"A: {machine.cpu.registers.A:X2}", 10, 50, 20, Raylib.BLACK);
            Raylib.DrawText($"X: {machine.cpu.registers.X:X2}", 10, 70, 20, Raylib.BLACK);
            Raylib.DrawText($"Y: {machine.cpu.registers.Y:X2}", 10, 90, 20, Raylib.BLACK);
            Raylib.DrawText($"PC: {machine.cpu.registers.PC:X4}", 10, 110, 20, Raylib.BLACK);
            Raylib.DrawText($"SP: {machine.cpu.registers.SP:X2}", 10, 130, 20, Raylib.BLACK);
            Raylib.DrawText($"Status: {machine.cpu.registers.P:X2}", 10, 150, 20, Raylib.BLACK);

            // Draw the CPU's speed, converting from Hz to MHz (with 6 decimal places)
            Raylib.DrawText($"Speed: {machine.CycleSpeed / 1000000.0:F6} MHz", 10, 170, 20, Raylib.BLACK);

            // Draw the machine's running state (checking machine.isRunning)
            Raylib.DrawText($"Running: {machine.isRunning}", 10, 190, 20, Raylib.BLACK);

            // Draw the current instruction
            Raylib.DrawText($"Instruction: {machine.cpu.currentDisassembly}", 10, 210, 20, Raylib.BLACK);

            // Draw the cycles remaining
            Raylib.DrawText($"Cycles Remaining: {machine.cpu.cycles}", 10, 230, 20, Raylib.BLACK);

            // Draw the paused state
            Raylib.DrawText($"Paused: {machine.isPaused}", 10, 250, 20, Raylib.BLACK);

            // Draw the single-stepping state
            Raylib.DrawText($"Single-Stepping: {machine.isSingleStepping}", 10, 270, 20, Raylib.BLACK);

            // Draw the memory at address 0x0200
            Raylib.DrawText($"Memory at 0x0200: {machine.cpu.memory.Read(0x0200):X2}", 10, 290, 20, Raylib.BLACK);
        }
    }
}
