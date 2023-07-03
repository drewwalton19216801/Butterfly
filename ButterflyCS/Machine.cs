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
    public static class Machine
    {
        /// <summary>
        /// The CPU timer.
        /// </summary>
        public static Timer? cpuTimer;

        /// <summary>
        /// The clock cycle duration, in seconds.
        /// </summary>
        public static double clockCycleDuration;

        /// <summary>
        /// Whether the machine is running.
        /// </summary>
        public static bool isRunning;

        /// <summary>
        /// Whether the machine is paused.
        /// </summary>
        public static bool isPaused;

        /// <summary>
        /// Whether the machine is single-stepping mode.
        /// </summary>
        public static bool isSingleStepping;

        /// <summary>
        /// Gets or sets the cycle speed.
        /// </summary>
        /// <remarks>Speed is in Hz.</remarks>
        public static double CycleSpeed { get; set; } = 1;

        /// <summary>
        /// Initializes the machine.
        /// </summary>
        public static void Init(CPU.Variant variant, double initialSpeed)
        {
            clockCycleDuration = 1 / CycleSpeed;
            isRunning = false;
            isPaused = false;
            isSingleStepping = false;

            // Set up the timer
            cpuTimer = new Timer(clockCycleDuration * 1000); // Timer interval is in milliseconds
            cpuTimer.Elapsed += Cycle!;

            // Set the CPU variant to whatever the user specified
            CPU.cpuVariant = variant;

            // Set the CPU speed to whatever the user specified
            CycleSpeed = initialSpeed;
        }

        /// <summary>
        /// Increases the speed of the machine.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void IncreaseSpeed(double amount)
        {
            // Increase the cycle speed by the specified amount
            CycleSpeed += amount;

            UpdateTimer();
        }

        /// <summary>
        /// Decreases the speed of the machine.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void DecreaseSpeed(double amount)
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
        public static void UpdateTimer()
        {
            if (cpuTimer == null) { throw new Exception("CPU timer is null."); }
            // Update the timer interval
            clockCycleDuration = 1 / CycleSpeed;
            cpuTimer.Interval = clockCycleDuration * 1000;
        }

        /// <summary>
        /// Runs a single machine cycle.
        /// </summary>
        public static void Cycle(object? sender, ElapsedEventArgs a)
        {
            // Execute one clock cycle if not paused or single-stepping
            if (!isPaused && !isSingleStepping)
            {
                lock (CPU.cpuLock)
                {
                    CPU.Clock();
                }
            }
        }

        /// <summary>
        /// Steps the machine.
        /// </summary>
        public static void Step()
        {
            if (isSingleStepping)
            {
                lock (CPU.cpuLock)
                {
                    CPU.Clock();
                }
            }
        }

        /// <summary>
        /// Runs the machine.
        /// </summary>
        public static void Run()
        {
            isRunning = true;
            isPaused = false;
            isSingleStepping = false;
        }

        /// <summary>
        /// Stops the machine.
        /// </summary>
        public static void Stop()
        {
            isRunning = false;
            isPaused = false;
            isSingleStepping = false;
        }

        /// <summary>
        /// Pauses the machine.
        /// </summary>
        public static void Pause()
        {
            isPaused = true;
            isSingleStepping = true;
        }

        /// <summary>
        /// Resets the machine to its initial power-up state.
        /// </summary>
        public static void Reset()
        {
            lock (CPU.cpuLock)
            {
                CPU.Reset();
            }
        }

        /// <summary>
        /// Peeks the memory at a specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>A string.</returns>
        public static string PeekMemory(ushort address)
        {
            byte data;
            string dataString;

            lock (CPU.cpuLock)
            {
                data = CPU.Read(address);
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
        public static string PeekRegister(string register)
        {
            byte data;
            string dataString;

            lock (CPU.cpuLock)
            {
                switch (register)
                {
                    case "A":
                        data = Registers.A;
                        break;
                    case "X":
                        data = Registers.X;
                        break;
                    case "Y":
                        data = Registers.Y;
                        break;
                    case "SP":
                        data = Registers.SP;
                        break;
                    case "P":
                        data = Registers.P;
                        break;
                    default:
                        data = 0;
                        break;
                }
            }

            // Convert the data to a string
            dataString = data.ToString("X2");

            return dataString;
        }

        /// <summary>
        /// Peeks the PC.
        /// </summary>
        /// <returns>A string.</returns>
        public static string PeekPC()
        {
            ushort data;
            string dataString;

            lock (CPU.cpuLock)
            {
                data = Registers.PC;
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
        public static void PokeMemory(ushort address, byte data)
        {
            lock (CPU.cpuLock)
            {
                CPU.Write(address, data);
            }
        }

        /// <summary>
        /// Pokes a register with the specified data.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <param name="data">The data.</param>
        public static void PokeRegister(string register, byte data)
        {
            lock (CPU.cpuLock)
            {
                switch (register)
                {
                    case "A":
                        Registers.A = data;
                        break;
                    case "X":
                        Registers.X = data;
                        break;
                    case "Y":
                        Registers.Y = data;
                        break;
                    case "SP":
                        Registers.SP = data;
                        break;
                    case "P":
                        Registers.P = data;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Pokes the PC with the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public static void PokePC(ushort data)
        {
            lock (CPU.cpuLock)
            {
                Registers.PC = data;
            }
        }

        /// <summary>
        /// Loads a program into memory.
        /// </summary>
        /// <remarks>
        /// This function reads a binary file into memory at the specified address.
        /// </remarks>
        public static void LoadProgram(string filename, ushort address)
        {
            byte[] program = File.ReadAllBytes(filename);

            for (int i = 0; i < program.Length; i++)
            {
                // If we've reached the end of the memory, stop loading the program
                if (address + i >= Memory.data.Length)
                {
                    break;
                }

                lock (CPU.cpuLock)
                {
                    Memory.data[address + i] = program[i];
                }
            }
        }
    }
}
