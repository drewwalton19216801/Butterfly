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
        private static readonly string subsystem = "Machine";

        /// <summary>
        /// The CPU.
        /// </summary>
        public CPU cpu;

        /// <summary>
        /// The CPU timer.
        /// </summary>
        public Timer cpuTimer;

        /// <summary>
        /// Gets or sets the machine cycle speed.
        /// </summary>
        public double CycleSpeed { get; set; } = 1; // 1 Hz = 1 cycle per second

        /// <summary>
        /// Duration of a single machine cycle, in seconds.
        /// </summary>
        public double clockCycleDuration;

        public bool isRunning;
        public bool isPaused;
        public bool isSingleStepping;

        /// <summary>
        /// Initializes a new instance of the <see cref="Machine"/> class.
        /// </summary>
        public Machine()
        {
            Log.Debug(subsystem, "Machine created.");
            cpu = new CPU();
            clockCycleDuration = 1 / CycleSpeed;
            isRunning = false;
            isPaused = false;
            isSingleStepping = false;

            // Set up the timer
            cpuTimer = new Timer(clockCycleDuration * 1000); // Timer interval is in milliseconds
            cpuTimer.Elapsed += Cycle!;
        }

        /// <summary>
        /// Increases the speed of the machine.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void IncreaseSpeed(double amount)
        {
            // Increase the cycle speed by the specified amount
            CycleSpeed += amount;

            UpdateTimer();

            // Log the new speed
            Log.Info(subsystem, $"Cycle speed increased to {CycleSpeed} Hz.");
        }

        /// <summary>
        /// Decreases the speed of the machine.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void DecreaseSpeed(double amount)
        {
            // Decrease the cycle speed by the specified amount, unless it's already 1 Hz
            if (CycleSpeed > 1)
            {
                CycleSpeed -= amount;
            }

            UpdateTimer();

            // Log the new speed
            Log.Info(subsystem, $"Cycle speed decreased to {CycleSpeed} Hz.");
        }

        /// <summary>
        /// Updates the timer.
        /// </summary>
        private void UpdateTimer()
        {
            // Update the timer interval
            clockCycleDuration = 1 / CycleSpeed;
            cpuTimer.Interval = clockCycleDuration * 1000;
        }

        /// <summary>
        /// Runs a single machine cycle.
        /// </summary>
        public void Cycle(object? sender, ElapsedEventArgs a)
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
