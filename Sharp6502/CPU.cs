namespace Sharp6502
{
    /// <summary>
    /// The 6502 microprocessor.
    /// </summary>
    public class CPU
    {
        /// <summary>
        /// The CPU variant (such as 6502, 65C02, etc).
        /// </summary>
        /// <remarks>
        /// Currently only a generic 6502 is supported.
        /// </remarks>
        public enum CPUVariant
        {
            Generic
        }

        public Registers registers = new Registers(0, 0, 0, 0xFD, 0, 0);
        public Memory memory = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CPU"/> class.
        /// </summary>
        public CPU()
        {
            Log.Info("CPU", "CPU created.");
        }

        /// <summary>
        /// Reads a byte from memory.
        /// </summary>
        /// <param name="address">The address to read from.</param>
        /// <returns>The data.</returns>
        private byte Read(ushort address)
        {
            return memory.Read(address);
        }

        /// <summary>
        /// Reads a word from memory.
        /// </summary>
        /// <param name="address">The address to read from.</param>
        /// <returns>The data.</returns>
        private ushort ReadWord(ushort address)
        {
            return (ushort)(Read(address) | (Read((ushort)(address + 1)) << 8));
        }

        /// <summary>
        /// Writes a byte to memory.
        /// </summary>
        /// <param name="address">The address to write to.</param>
        /// <param name="data">The data to write.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Write(ushort address, byte data)
        {
            memory.Write(address, data);
        }

        /// <summary>
        /// Resets the CPU to its initial power-up state.
        /// </summary>
        public void Reset()
        {
            Log.Info("CPU", "Resetting CPU to power-up state.");
            registers.A = 0;
            registers.X = 0;
            registers.Y = 0;
            registers.SP = 0xFD;
            registers.PC = ReadWord(0xFFFC); // Read the PC from the reset vector.
            registers.P = (byte)(Registers.Flags.None | Registers.Flags.Unused);
            Log.Info("CPU", "CPU reset complete.");
        }
    }
}