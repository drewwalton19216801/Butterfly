using Sharp6502;

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
        public static string subsystem = "Machine";

        /// <summary>
        /// The CPU.
        /// </summary>
        public CPU cpu = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Machine"/> class.
        /// </summary>
        public Machine()
        {
            Log.Info(subsystem, "Machine created.");
        }

        /// <summary>
        /// Resets the machine to its initial power-up state.
        /// </summary>
        public void Reset()
        {
            Log.Info(subsystem, "Resetting machine to power-up state.");
            cpu.Reset();
            Log.Info(subsystem, "Machine reset complete.");
        }

        /// <summary>
        /// Loads a program into memory.
        /// </summary>
        /// <remarks>
        /// This function reads a binary file into memory at the specified address.
        /// </remarks>
        public void LoadProgram(string filename, ushort address)
        {
            Log.Info(subsystem, $"Loading program {filename} into memory at address {address:X4}.");
            byte[] program = File.ReadAllBytes(filename);
            for (int i = 0; i < program.Length; i++)
            {
                cpu.memory.data[address + i] = program[i];
            }
            Log.Info(subsystem, $"Program {filename} loaded into memory at address {address:X4}.");
        }
    }
}
