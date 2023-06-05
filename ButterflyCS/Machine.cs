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
            Log.Debug(subsystem, "Machine created.");
        }

        /// <summary>
        /// Runs a single machine cycle.
        /// </summary>
        public void Cycle()
        {
            Log.Debug(subsystem, "Running a single machine cycle.");
            cpu.Clock();
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

            // Make sure the IRQ vector points to the BRK instruction.
            cpu.memory.data[0xFFFE] = 0x06;
            cpu.memory.data[0xFFFF] = 0x80;

            // Make sure the NMI vector points to the BRK instruction.
            cpu.memory.data[0xFFFA] = 0x06;
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
