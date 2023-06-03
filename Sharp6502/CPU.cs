namespace Sharp6502
{
    /// <summary>
    /// The 6502 microprocessor.
    /// </summary>
    public class CPU
    {
        public static Registers registers = new Registers(0, 0, 0, 0xFD, 0, 0);

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
            registers.PC = 0;
            registers.P = (byte)Registers.Flags.None;
            Log.Info("CPU", "CPU reset complete.");
        }
    }
}