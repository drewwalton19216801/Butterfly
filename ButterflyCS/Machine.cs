using System;
using System.Reflection.PortableExecutable;
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


        public CPU cpu; // The CPU
        public Timer cpuTimer; // The CPU timer
        public double clockCycleDuration; // The duration of a clock cycle, in seconds
        public bool isRunning; // Whether the machine is running
        public bool isPaused; // Whether the machine is paused
        public bool isSingleStepping; // Whether the machine is single-stepping

        /// <summary>
        /// Gets or sets the cycle speed.
        /// </summary>
        public double CycleSpeed { get; set; } = 1; // The cycle speed, in Hz

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
                lock (cpu.cpuLock)
                {
                    cpu.Clock();
                }
            }
        }

        /// <summary>
        /// Resets the machine to its initial power-up state.
        /// </summary>
        public void Reset()
        {
            Log.Debug(subsystem, "Resetting machine to power-up state.");
            lock (cpu.cpuLock)
            {
                cpu.Reset();
            }
            Log.Debug(subsystem, "Machine reset complete.");
        }

        /// <summary>
        /// Initializes the machine.
        /// </summary>
        /// <param name="romFilePath">The rom file path.</param>
        public void Init(string romFilePath, CPU.Variant variant, double initialSpeed)
        {
            Log.Info(subsystem, "Machine initializing");

            // Initialize the logging subsystem
            Log.EnableDebugMessages();

            // Load the ROM
            LoadProgram(romFilePath, 0x8000);

            // Set the CPU variant to whatever the user specified
            cpu.cpuVariant = variant;

            // Set the CPU speed to whatever the user specified
            CycleSpeed = initialSpeed;

            Log.Info(subsystem, "Machine initialized");
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

            // Log the size of the program
            Log.Debug(subsystem, $"Program {filename} is {program.Length} bytes long.");

            for (int i = 0; i < program.Length; i++)
            {
                // If we've reached the end of the memory, stop loading the program
                if (address + i >= cpu.memory.data.Length)
                {
                    Log.Warning(subsystem, $"Program {filename} is too large to fit in memory at address {address:X4}.");
                    break;
                }

                lock (cpu.cpuLock)
                {
                    cpu.memory.data[address + i] = program[i];
                }
            }
            Log.Debug(subsystem, $"Program {filename} loaded into memory at address {address:X4}.");
        }
    }
}
