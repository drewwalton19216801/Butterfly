namespace Sharp6502
{
    /// <summary>
    /// The status flag bitmasks.
    /// </summary>
    /// <remarks>
    /// These are the bitmasks for the status flags in the P register. They
    /// are used to set and clear the flags.
    /// </remarks>
    public enum CPUFlags : byte
    {
        /// <summary>
        /// No flags set.
        /// </summary>
        None = 0,

        /// <summary>
        /// The carry flag.
        /// </summary>
        Carry = 1 << 0,

        /// <summary>
        /// The zero flag.
        /// </summary>
        Zero = 1 << 1,

        /// <summary>
        /// The interrupt disable flag.
        /// </summary>
        InterruptDisable = 1 << 2,

        /// <summary>
        /// The decimal flag.
        /// </summary>
        Decimal = 1 << 3,

        /// <summary>
        /// The break flag.
        /// </summary>
        Break = 1 << 4,

        /// <summary>
        /// The unused flag.
        /// </summary>
        Unused = 1 << 5,

        /// <summary>
        /// The overflow flag.
        /// </summary>
        Overflow = 1 << 6,

        /// <summary>
        /// The negative flag.
        /// </summary>
        Negative = 1 << 7
    }

    /// <summary>
    /// The 6502 microprocessor registers.
    /// </summary>
    public class Registers
    {
        /// <summary>
        /// The A (accumulator) register.
        /// </summary>
        public byte A { get; set; }

        /// <summary>
        /// The X index register.
        /// </summary>
        public byte X { get; set; }

        /// <summary>
        /// The Y index register.
        /// </summary>
        public byte Y { get; set; }

        /// <summary>
        /// The SP (stack pointer) register.
        /// </summary>
        public byte SP { get; set; }

        /// <summary>
        /// The PC (program counter) register.
        /// </summary>
        public ushort PC { get; set; }

        /// <summary>
        /// The P (processor status) register.
        /// </summary>
        public byte P { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Registers"/> class.
        /// </summary>
        public Registers(byte a, byte x, byte y, byte sp, ushort pc, byte p)
        {
            A = a;
            X = x;
            Y = y;
            SP = sp;
            PC = pc;
            P = p;
        }

        /// <summary>
        /// Sets the value of a status flag.
        /// </summary>
        /// <param name="flag">The flag to set.</param>
        /// <param name="value">The value to set the flag to.</param>
        public void SetFlag(CPUFlags flag, bool value)
        {
            if (value)
            {
                P |= (byte)flag;
            }
            else
            {
                P &= (byte)~flag;
            }
        }

        /// <summary>
        /// Gets the value of a status flag.
        /// </summary>
        /// <param name="flag">The flag to get.</param>
        /// <returns>The flag value.</returns>
        public bool GetFlag(CPUFlags flag)
        {
            return (P & (byte)flag) != 0;
        }
    }
}
