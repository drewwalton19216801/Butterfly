using System.Runtime.CompilerServices;
using System.Text;

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
        public byte cycles = 0;
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
            Log.Debug("CPU", "CPU created.");
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
        /// Runs the CPU for one clock cycle.
        /// </summary>
        public void Clock()
        {
            // Log the current cycles
            Log.Debug("CPU", $"Cycles: {cycles}");

            /*
             * Instructions can require a variable number of clock cycles to execute. The number of cycles required is stored in the instruction's
             * definition. The number of cycles remaining is stored in the CPU's cycles field. When the cycles field reaches zero, the instruction
             * is complete and the next instruction can be fetched.
             */
            if (cycles == 0)
            {
                // Set the CPU state to fetching
                cpuState = ExecutionState.Fetching;

                // Log the execution state
                Log.Debug("CPU", "Fetching instruction...");

                /*
                 * Read the next instruction opcode from memory. We can then use
                 * this value to look up the instruction definition.
                 */
                opcode = Read(registers.PC);

                Log.Debug("CPU", $"Opcode: {opcode:X2}");

                // Always set the unused flag to 1.
                registers.SetFlag(Registers.Flags.Unused, true);

                // Decode the instruction
                CurrentInstruction = InstructionSet.Decode(opcode);

                // Get the number of cycles required to execute the instruction
                cycles = CurrentInstruction.Cycles;

                // Set the CPU state to executing
                cpuState = ExecutionState.Executing;

                // Log the disassembly of the current instruction
                Log.Debug("CPU", $"Disassembly: {Disassemble(CurrentInstruction)}");

                // Increment the program counter
                registers.PC++;

                // Run the addressing mode method and get the number of additional cycles required
                byte addressingUsedExtraCycle = AddressingModes.GetAddress(this, CurrentInstruction.AddressingMode);

                // Run the instruction method and get the number of additional cycles required
                byte instructionUsedExtraCycle = InstructionExecutor.ExecuteInstruction(this);

                // Add the additional cycles to the total number of cycles required
                cycles += (byte)(addressingUsedExtraCycle & instructionUsedExtraCycle);

                // Set the unused flag to 1
                registers.SetFlag(Registers.Flags.Unused, true);
            }

            // Decrement the number of cycles remaining for this instruction
            cycles--;
        }

        /// <summary>
        /// Emits an interrupt request.
        /// </summary>
        public void IRQ()
        {
            // Make sure interrupts are enabled
            if (registers.GetFlag(Registers.Flags.InterruptDisable) == false)
            {
                // Push the program counter to the stack
                Write((ushort)(0x0100 + registers.SP), (byte)((registers.PC >> 8) & 0x00FF));
                registers.SP--;
                Write((ushort)(0x0100 + registers.SP), (byte)(registers.PC & 0x00FF));
                registers.SP--;

                // Push the status register to the stack
                registers.SetFlag(Registers.Flags.Break, false);
                registers.SetFlag(Registers.Flags.Unused, true);
                registers.SetFlag(Registers.Flags.InterruptDisable, true);
                Write((ushort)(0x0100 + registers.SP), registers.P);
                registers.SP--;

                // Set the interrupt vector
                registers.PC = ReadWord(0xFFFE);

                // Set the CPU state to interrupt
                cpuState = ExecutionState.Interrupt;

                // Add the number of cycles required to execute the interrupt
                cycles += 7;
            }   
        }

        /// <summary>
        /// Emits a non-maskable interrupt request.
        /// </summary>
        public void NMI()
        {
            // Push the program counter to the stack
            Write((ushort)(0x0100 + registers.SP), (byte)((registers.PC >> 8) & 0x00FF));
            registers.SP--;
            Write((ushort)(0x0100 + registers.SP), (byte)(registers.PC & 0x00FF));
            registers.SP--;

            // Set the break flag to 0, unused and interrupt to 1
            registers.SetFlag(Registers.Flags.Break, false);
            registers.SetFlag(Registers.Flags.Unused, true);
            registers.SetFlag(Registers.Flags.InterruptDisable, true);

            // Push the status register to the stack
            Write((ushort)(0x0100 + registers.SP), registers.P);

            // Decrement the stack pointer
            registers.SP--;

            // Set the interrupt vector
            registers.PC = ReadWord(0xFFFA);
            cpuState = ExecutionState.Interrupt;

            // Add the number of cycles required to execute the interrupt
            cycles += 8;
        }

        /// <summary>
        /// Pushes a byte to the stack
        /// </summary>
        /// <param name="data">The data.</param>
        public void PushStack(byte data)
        {
            Write((ushort)(0x0100 + registers.SP), data);
            registers.SP--;
        }

        /// <summary>
        /// Fetches the next byte from memory.
        /// </summary>
        /// <returns>The data.</returns>
        /// <remarks>
        /// This method fetches the next byte from memory. If the current instruction is an implied instruction, no byte is fetched.
        /// </remarks>
        public byte Fetch()
        {
            if (CurrentInstruction == null)
            {
                throw new InvalidOperationException("Cannot fetch instruction when no instruction is set.");
            } else if (!(CurrentInstruction.AddressingMode == Addressing.Implied))
            {
                fetchedByte = Read(addressAbsolute);
            }
            return fetchedByte;
        }

        /// <summary>
        /// Resets the CPU to its initial power-up state.
        /// </summary>
        public void Reset()
        {
            Log.Debug("CPU_RESET", "Resetting CPU to power-up state.");
            registers.A = 0;
            registers.X = 0;
            registers.Y = 0;
            registers.SP = 0xFD;
            registers.PC = ReadWord(0xFFFC); // Read the PC from the reset vector.
            registers.P = (byte)(Registers.Flags.None | Registers.Flags.Unused);
            cycles = 8;
            Log.Debug("CPU_RESET", "CPU reset complete.");

            // assemble a string representing the current register values (except the status register)
            string registersString = string.Format("A:{0:X2} X:{1:X2} Y:{2:X2} SP:{3:X2} PC:{4:X4}", registers.A, registers.X, registers.Y, registers.SP, registers.PC);
            // now assemble a string representing the current status register values as individual bits
            string statusString = string.Format("NV-BDIZC", registers.GetFlag(Registers.Flags.Negative) ? 'N' : 'n',
                registers.GetFlag(Registers.Flags.Overflow) ? 'V' : 'v',
                registers.GetFlag(Registers.Flags.Unused) ? 'U' : 'u',
                registers.GetFlag(Registers.Flags.Break) ? 'B' : 'b',
                registers.GetFlag(Registers.Flags.Decimal) ? 'D' : 'd',
                registers.GetFlag(Registers.Flags.InterruptDisable) ? 'I' : 'i',
                registers.GetFlag(Registers.Flags.Zero) ? 'Z' : 'z',
                registers.GetFlag(Registers.Flags.Carry) ? 'C' : 'c');

            // Assemble the final string, with the register values and status register bits on separate lines
            string finalString = string.Format("{0} {1}", registersString, statusString);

            Log.Debug("CPU_RESET_FINISH", finalString);
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