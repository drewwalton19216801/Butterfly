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
        /// A demo device that hooks into the memory space.
        /// </summary>
        public MemHookDemoDevice memHookDemoDevice;

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
            memHookDemoDevice = new MemHookDemoDevice();
            clockCycleDuration = 1 / CycleSpeed;
            isRunning = false;
            isPaused = false;
            isSingleStepping = false;

            // Set up the timer
            cpuTimer = new Timer(clockCycleDuration * 1000); // Timer interval is in milliseconds
            cpuTimer.Elapsed += Cycle!;

            // Hook up the demo device
            cpu.memory.RegisterReadHook(memHookDemoDevice.startAddress, memHookDemoDevice.endAddress, memHookDemoDevice.Read); // Read hook
            cpu.memory.RegisterWriteHook(memHookDemoDevice.startAddress, memHookDemoDevice.endAddress, memHookDemoDevice.Write); // Write hook
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

                cpu.memory.data[address + i] = program[i];
            }
            Log.Debug(subsystem, $"Program {filename} loaded into memory at address {address:X4}.");
        }
    }
}
