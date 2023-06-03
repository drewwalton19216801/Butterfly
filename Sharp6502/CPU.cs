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
        public enum Variant
        {
            Generic
        }

        /// <summary>
        /// The CPU execution state.
        /// </summary>
        public enum ExecutionState
        {
            Stopped,
            Fetching,
            Executing,
            Interrupt,
            IllegalOpcode
        }

        public Registers registers = new(0, 0, 0, 0xFD, 0, 0);
        public Memory memory = new();
        public byte cycles = 0x00;
        public ushort addressAbsolute = 0x0000;
        public ushort addressRelative = 0x00;
        public byte opcode = 0x00;
        public ExecutionState cpuState = ExecutionState.Stopped;
        public Variant cpuVariant = Variant.Generic;
        public byte fetchedByte = 0x00;
        

        /// <summary>
        /// Gets or sets the clock speed.
        /// </summary>
        public double clockSpeed { get; set; } = 0; // Hz

        /// <summary>
        /// Gets or sets the current instruction.
        /// </summary>
        public Instruction? CurrentInstruction { get; set; }

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

        public void Clock()
        {

        }

        public void IRQ()
        {
        }

        public void NMI()
        {
        }

        public byte Fetch()
        {
            return 0;
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

        /// <summary>
        /// Disassembles the instruction at the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>Ths disassembled instruction.</returns>
        public string Disassemble(ushort address)
        {
            // Read the instruction at the specified address.
            byte opcode = Read(address);

            // Get the instruction.
            Instruction instruction = InstructionSet.Decode(opcode);

            // Return the disassembled instruction.
            return Disassemble(instruction);
        }

        /// <summary>
        /// Disassembles an instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <returns>The disassembled instruction.</returns>
        public string Disassemble(Instruction instruction)
        {
            // Get the instruction's address mode.
            Addressing addressingMode = instruction.AddressingMode;

            ushort operand;
            string operandString = string.Empty;

            // Get the string representation of the operand.
            switch (addressingMode)
            {
                case Addressing.Immediate:
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"#${operand:X2}";
                    break;
                case Addressing.ZeroPage:
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"${operand:X2}";
                    break;
                case Addressing.ZeroPageX:
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"${operand:X2},X";
                    break;
                case Addressing.ZeroPageY:
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"${operand:X2},Y";
                    break;
                case Addressing.Absolute:
                    operand = ReadWord((ushort)(registers.PC + 1));
                    operandString = $"${operand:X4}";
                    break;
                case Addressing.AbsoluteX:
                    operand = ReadWord((ushort)(registers.PC + 1));
                    operandString = $"${operand:X4},X";
                    break;
                case Addressing.AbsoluteY:
                    operand = ReadWord((ushort)(registers.PC + 1));
                    operandString = $"${operand:X4},Y";
                    break;
                case Addressing.Indirect:
                    operand = ReadWord((ushort)(registers.PC + 1));
                    operandString = $"(${operand:X4})";
                    break;
                case Addressing.IndirectX:
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"(${operand:X2},X)";
                    break;
                case Addressing.IndirectY:
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"(${operand:X2}),Y";
                    break;
                case Addressing.Relative:
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"${operand:X2}";
                    break;
                case Addressing.Accumulator:
                    operandString = "A";
                    break;
            }

            // Return the disassembled instruction.
            return $"{instruction.Name} {operandString}";
        }
    }
}