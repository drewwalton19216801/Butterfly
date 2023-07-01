using System.Timers;
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
        /// Peeks the memory at a specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>A string.</returns>
        public string PeekMemory(ushort address)
        {
            byte data;
            string dataString;

            lock (cpu.cpuLock)
            {
                data = cpu.Read(address);
            }

            // Convert the data to a string
            dataString = data.ToString("X2");

            return dataString;
        }

        /// <summary>
        /// Peeks at a register.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns>A string.</returns>
        public string PeekRegister(string register)
        {
            byte data;
            string dataString;

            switch (register)
            {
                case "A":
                    lock (cpu.cpuLock)
                    {
                        data = cpu.registers.A;
                    }
                    break;
                case "X":
                    lock (cpu.cpuLock)
                    {
                        data = cpu.registers.X;
                    }
                    break;
                case "Y":
                    lock (cpu.cpuLock)
                    {
                        data = cpu.registers.Y;
                    }
                    break;
                case "SP":
                    lock (cpu.cpuLock)
                    {
                        data = cpu.registers.SP;
                    }
                    break;
                case "P":
                    lock (cpu.cpuLock)
                    {
                        data = cpu.registers.P;
                    }
                    break;
                default:
                    data = 0;
                    break;
            }

            // Convert the data to a string
            dataString = data.ToString("X2");

            return dataString;
        }

        /// <summary>
        /// Peeks the PC.
        /// </summary>
        /// <returns>A string.</returns>
        public string PeekPC()
        {
            ushort data;
            string dataString;

            lock (cpu.cpuLock)
            {
                data = cpu.registers.PC;
            }

            // Convert the data to a string
            dataString = data.ToString("X4");

            return dataString;
        }

        /// <summary>
        /// Pokes the memory at the specified address with the specified data.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="data">The data.</param>
        public void PokeMemory(ushort address, byte data)
        {
            lock (cpu.cpuLock)
            {
                cpu.Write(address, data);
            }
        }

        /// <summary>
        /// Pokes a register with the specified data.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <param name="data">The data.</param>
        public void PokeRegister(string register, byte data)
        {
            switch (register)
            {
                case "A":
                    lock (cpu.cpuLock)
                    {
                        cpu.registers.A = data;
                    }
                    break;
                case "X":
                    lock (cpu.cpuLock)
                    {
                        cpu.registers.X = data;
                    }
                    break;
                case "Y":
                    lock (cpu.cpuLock)
                    {
                        cpu.registers.Y = data;
                    }
                    break;
                case "SP":
                    lock (cpu.cpuLock)
                    {
                        cpu.registers.SP = data;
                    }
                    break;
                case "P":
                    lock (cpu.cpuLock)
                    {
                        cpu.registers.P = data;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Pokes the PC with the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void PokePC(ushort data)
        {
            lock (cpu.cpuLock)
            {
                cpu.registers.PC = data;
            }
        }

        /// <summary>
        /// Initializes the machine.
        /// </summary>
        /// <param name="romFilePath">The rom file path.</param>
        public void Init(string romFilePath, CPU.Variant variant, double initialSpeed)
        {
            // Load the ROM
            LoadProgram(romFilePath, 0x8000);

            // Set the CPU variant to whatever the user specified
            cpu.cpuVariant = variant;

            // Set the CPU speed to whatever the user specified
            CycleSpeed = initialSpeed;
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
