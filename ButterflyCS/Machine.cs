using System;
using System.Timers;
using Raylib_CsLo;
using Sharp6502;
using Timer = System.Timers.Timer;

namespace ButterflyCS
{
    /// <summary>
    /// The machine object.
    /// </summary>
    /// <remarks>
    /// Implements a 6502-based machine.
    /// </remarks>
    public class Machine
    {
        /// <summary>
        /// The subsystem.
        /// </summary>
        private static string subsystem = "Machine";

        /// <summary>
        /// The CPU.
        /// </summary>
        private CPU cpu;

        /// <summary>
        /// The CPU timer.
        /// </summary>
        private Timer cpuTimer;

        /// <summary>
        /// Gets or sets the machine cycle speed.
        /// </summary>
        private double cycleSpeed { get; set; } = 1; // 1 Hz = 1 cycle per second

        /// <summary>
        /// Duration of a single machine cycle, in seconds.
        /// </summary>
        private double clockCycleDuration;

        private bool isRunning;
        private bool isPaused;
        private bool isSingleStepping;

        /// <summary>
        /// Initializes a new instance of the <see cref="Machine"/> class.
        /// </summary>
        public Machine()
        {
            Log.Debug(subsystem, "Machine created.");
            cpu = new CPU();
            clockCycleDuration = 1 / cycleSpeed;
            isRunning = false;
            isPaused = false;
            isSingleStepping = false;

            // Set up the timer
            cpuTimer = new Timer(clockCycleDuration * 1000); // Timer interval is in milliseconds
            cpuTimer.Elapsed += Cycle!;
        }

        /// <summary>
        /// Starts the machine.
        /// </summary>
        public void Start()
        {
            Raylib.InitWindow(800, 600, "ButterflyCS");
            Raylib.SetTargetFPS(60);

            isRunning = false;
            isPaused = true;
            isSingleStepping = false;

            // Start the CPU timer
            cpuTimer.Start();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                // TODO: add display logic here
                DrawMachineState();
                Raylib.EndDrawing();

                // Check for pause (Ctrl+P)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_P))
                {
                    isPaused = !isPaused;
                    isSingleStepping = false;
                }
                // Check for single-step (Ctrl+S)
                else if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                {
                    isSingleStepping = true;
                    isPaused = true;
                }

                // Check for speed increase (Ctrl+PgUp)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyDown(KeyboardKey.KEY_PAGE_UP))
                {
                    // Increase the cycle speed by 1 Hz
                    cycleSpeed += 1;

                    // Update the timer interval
                    clockCycleDuration = 1 / cycleSpeed;
                    cpuTimer.Interval = clockCycleDuration * 1000;

                    // Log the new speed
                    Log.Debug(subsystem, $"Cycle speed increased to {cycleSpeed} Hz.");
                }

                // Check for speed decrease (Ctrl+PgDown)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyDown(KeyboardKey.KEY_PAGE_DOWN))
                {
                    // Decrease the cycle speed by 1 Hz, unless it's already 1 Hz
                    if (cycleSpeed > 1)
                    {
                        cycleSpeed -= 1;
                    }

                    // Update the timer interval
                    clockCycleDuration = 1 / cycleSpeed;
                    cpuTimer.Interval = clockCycleDuration * 1000;

                    // Log the new speed
                    Log.Debug(subsystem, $"Cycle speed decreased to {cycleSpeed} Hz.");
                }

                // Check for reset (Ctrl+R)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_R))
                {
                    Log.Debug(subsystem, "Resetting machine.");
                    Reset();
                }

                // Check for quit (Ctrl+Q)
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
                {
                    Log.Debug(subsystem, "Quitting.");
                    break;
                }

                // Single-step if paused and single-stepping is enabled, and the space bar is pressed
                if (isPaused && isSingleStepping && Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    Log.Debug(subsystem, "Single-stepping.");
                    cpu.Clock();
                }
            }

            Raylib.CloseWindow();
        }

        /// <summary>
        /// Draw the machine's state.
        /// </summary>
        private void DrawMachineState()
        {
            Raylib.ClearBackground(Raylib.GREEN);
            Raylib.DrawFPS(10, 10);
            // Draw the machine's state
            Raylib.DrawText("Machine State", 10, 30, 20, Raylib.BLACK);
            Raylib.DrawText($"A: {cpu.registers.A:X2}", 10, 50, 20, Raylib.BLACK);
            Raylib.DrawText($"X: {cpu.registers.X:X2}", 10, 70, 20, Raylib.BLACK);
            Raylib.DrawText($"Y: {cpu.registers.Y:X2}", 10, 90, 20, Raylib.BLACK);
            Raylib.DrawText($"PC: {cpu.registers.PC:X4}", 10, 110, 20, Raylib.BLACK);
            Raylib.DrawText($"SP: {cpu.registers.SP:X2}", 10, 130, 20, Raylib.BLACK);
            Raylib.DrawText($"Status: {cpu.registers.P:X2}", 10, 150, 20, Raylib.BLACK);

            // Draw the CPU's speed, converting from Hz to MHz (with 6 decimal places)
            Raylib.DrawText("CPU Speed", 10, 170, 20, Raylib.BLACK);
            Raylib.DrawText($"{cycleSpeed / 1000000:F6} MHz", 10, 190, 20, Raylib.BLACK);

            // Draw the machine's running state
            Raylib.DrawText("Machine Running", 10, 210, 20, Raylib.BLACK);
            Raylib.DrawText(isRunning ? "Yes" : "No", 10, 230, 20, Raylib.BLACK);

            // Draw the current instruction
            Raylib.DrawText("Current Instruction", 10, 250, 20, Raylib.BLACK);
            Raylib.DrawText(cpu.currentDisassembly, 10, 270, 20, Raylib.BLACK);

            // Draw the cycles remaining
            Raylib.DrawText("Cycles Remaining", 10, 290, 20, Raylib.BLACK);
            Raylib.DrawText(cpu.cycles.ToString(), 10, 310, 20, Raylib.BLACK);

            // Draw the paused state
            Raylib.DrawText("Paused", 10, 330, 20, Raylib.BLACK);
            Raylib.DrawText(isPaused ? "Yes" : "No", 10, 350, 20, Raylib.BLACK);

            // Draw the single-stepping state
            Raylib.DrawText("Single-Stepping", 10, 370, 20, Raylib.BLACK);
            Raylib.DrawText(isSingleStepping ? "Yes" : "No", 10, 390, 20, Raylib.BLACK);
        }

        /// <summary>
        /// Runs a single machine cycle.
        /// </summary>
        private void Cycle(object? sender, ElapsedEventArgs a)
        {
            // Execute one clock cycle if not paused or single-stepping
            if (!isPaused && !isSingleStepping)
            {
                Log.Debug(subsystem, "Running a single machine cycle.");
                cpu.Clock();
            }
        }

        /// <summary>
        /// Resets the machine to its initial power-up state.
        /// </summary>
        public void Reset()
        {
            Log.Debug(subsystem, "Resetting machine to power-up state.");
            cpu.Reset();
            Log.Debug(subsystem, "Machine reset complete.");
        }

        /// <summary>
        /// Loads the demo program.
        /// </summary>
        public void LoadDemoProgram()
        {
            /* Load the following program into memory at address 0x8000:
             * LDA #$01
             * STA $0200
             * NOP
             * BRK
            */
            byte[] program = new byte[] { 0xA9, 0x01, 0x8D, 0x00, 0x02, 0xEA, 0x00 };

            for (int i = 0; i < program.Length; i++)
            {
                cpu.memory.data[0x8000 + i] = program[i];
            }

            // Make sure the reset vector points to the start of the program.
            cpu.memory.data[0xFFFC] = 0x00;
            cpu.memory.data[0xFFFD] = 0x80;

            // Make sure the IRQ vector points to the initial instruction. This is
            // normally useless, but we don't have proper branching implemented yet,
            // so we're using the BRK instruction to restart the program.
            cpu.memory.data[0xFFFE] = 0x00;
            cpu.memory.data[0xFFFF] = 0x80;

            // Same as above, but for the NMI vector.
            cpu.memory.data[0xFFFA] = 0x00;
            cpu.memory.data[0xFFFB] = 0x80;
        }

        /// <summary>
        /// Loads a program into memory.
        /// </summary>
        /// <remarks>
        /// This function reads a binary file into memory at the specified address.
        /// </remarks>
        public void LoadProgram(string filename, ushort address)
        {
            Log.Debug(subsystem, $"Loading program {filename} into memory at address {address:X4}.");
            byte[] program = File.ReadAllBytes(filename);
            for (int i = 0; i < program.Length; i++)
            {
                cpu.memory.data[address + i] = program[i];
            }
            Log.Debug(subsystem, $"Program {filename} loaded into memory at address {address:X4}.");
        }
    }
}
