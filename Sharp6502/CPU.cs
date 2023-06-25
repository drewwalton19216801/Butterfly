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
        /// The subsystem.
        /// </summary>
        private static readonly string subsystem = "CPU";

        /// <summary>
        /// The CPU variant (such as 6502, 65C02, etc).
        /// </summary>
        public enum Variant
        {
            NMOS_6502, // The original 6502
            CMOS_65C02, // The 65C02
            NES_6502, // The NES 6502 is a 6502 with a few quirks (aka Ricoh 2A03)
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

        public Registers registers = new(0, 0, 0, 0xFF, 0, 0); // The CPU registers
        public Memory memory = new(); // The CPU memory
        public byte cycles = 0; // The number of cycles remaining for the current instruction
        public ushort temp = 0x0000; // A variable to temporarily store data
        public ushort addressAbsolute = 0x0000; // Represents the absolute address fetched
        public ushort addressRelative = 0x00; // Represents the relative address fetched
        public byte opcode = 0x00; // The current opcode
        public ExecutionState cpuState = ExecutionState.Stopped; // The current execution state
        public Variant cpuVariant = Variant.CMOS_65C02; // The CPU variant (default is WDC 65C02)
        public byte fetchedByte = 0x00; // The fetched byte
        public string currentDisassembly = string.Empty; // The current disassembly

        /// <summary>
        /// Gets or sets the current instruction.
        /// </summary>
        public Instruction? CurrentInstruction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CPU"/> class.
        /// </summary>
        public CPU()
        {
            Log.Debug(subsystem, "CPU created.");
        }

        /// <summary>
        /// Reads a byte from memory.
        /// </summary>
        /// <param name="address">The address to read from.</param>
        /// <returns>The data.</returns>
        public byte Read(ushort address)
        {
            return memory.Read(address);
        }

        /// <summary>
        /// Reads a word from memory.
        /// </summary>
        /// <param name="address">The address to read from.</param>
        /// <returns>The data.</returns>
        public ushort ReadWord(ushort address)
        {
            return (ushort)(Read(address) | (Read((ushort)(address + 1)) << 8));
        }

        /// <summary>
        /// Writes a byte to memory.
        /// </summary>
        /// <param name="address">The address to write to.</param>
        /// <param name="data">The data to write.</param>
        public void Write(ushort address, byte data)
        {
            memory.Write(address, data);
        }

        /// <summary>
        /// Runs the CPU for one clock cycle.
        /// </summary>
        public void Clock()
        {
            // Log the current cycles
            Log.Debug(subsystem, $"Cycles: {cycles}");

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
                Log.Debug(subsystem, "Fetching instruction...");

                /*
                 * Read the next instruction opcode from memory. We can then use
                 * this value to look up the instruction definition.
                 */
                opcode = Read(registers.PC);

                Log.Debug(subsystem, $"Opcode: {opcode:X2}");

                // Always set the unused flag to 1.
                registers.SetFlag(CPUFlags.Unused, true);

                // Decode the instruction
                CurrentInstruction = InstructionSet.Decode(opcode);

                // Get the number of cycles required to execute the instruction
                cycles = CurrentInstruction.Cycles;

                // Set the CPU state to executing
                cpuState = ExecutionState.Executing;

                // Update the current disassembly
                currentDisassembly = Disassemble(CurrentInstruction);

                // Log the disassembly of the current instruction
                Log.Debug(subsystem, $"Disassembly: {currentDisassembly}");

                // Increment the program counter
                registers.PC++;

                // Run the addressing mode method and get the number of additional cycles required
                byte addressingUsedExtraCycle = AddressingModes.GetAddress(this, CurrentInstruction.AddressingMode);

                // Run the instruction method and get the number of additional cycles required
                byte instructionUsedExtraCycle = InstructionExecutor.ExecuteInstruction(this);

                // Add the additional cycles to the total number of cycles required
                cycles += (byte)(addressingUsedExtraCycle & instructionUsedExtraCycle);

                // Set the unused flag to 1
                registers.SetFlag(CPUFlags.Unused, true);
            }

            // Decrement the number of cycles remaining for this instruction
            cycles--;
        }

        /// <summary>
        /// Emits an interrupt request.
        /// </summary>
        /// <remarks>
        /// IRQs are a complicated operation. The current instruction is allowed to finish, but then the current program counter and status register
        /// are pushed to the stack. The interrupt vector is then read from memory and the program counter is set to the interrupt vector. The interrupt
        /// flag is set to prevent further interrupts from being processed. When the interrupt handler is complete, the status register and program
        /// counter are restored to their previous values and execution continues as normal.
        /// </remarks>
        public void IRQ()
        {
            // Make sure interrupts are enabled
            if (registers.GetFlag(CPUFlags.InterruptDisable) == false)
            {
                // Push the program counter to the stack
                Write((ushort)(0x0100 + registers.SP), (byte)((registers.PC >> 8) & 0x00FF));
                registers.SP--;
                Write((ushort)(0x0100 + registers.SP), (byte)(registers.PC & 0x00FF));
                registers.SP--;

                // Push the status register to the stack
                registers.SetFlag(CPUFlags.Break, false);
                registers.SetFlag(CPUFlags.Unused, true);
                registers.SetFlag(CPUFlags.InterruptDisable, true);
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
        /// <remarks>
        /// A non-maskable interrupt is similar to a regular IRQ, but it cannot be ignored. It
        /// acts like a regular IRQ, but reads the new PC from memory location 0xFFFA instead of
        /// 0xFFFE.
        /// </remarks>
        public void NMI()
        {
            // Push the program counter to the stack
            Write((ushort)(0x0100 + registers.SP), (byte)((registers.PC >> 8) & 0x00FF));
            registers.SP--;
            Write((ushort)(0x0100 + registers.SP), (byte)(registers.PC & 0x00FF));
            registers.SP--;

            // Set the break flag to 0, unused and interrupt to 1
            registers.SetFlag(CPUFlags.Break, false);
            registers.SetFlag(CPUFlags.Unused, true);
            registers.SetFlag(CPUFlags.InterruptDisable, true);

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
        public void PushByte(byte data)
        {
            Write((ushort)(0x0100 + registers.SP), data);
            registers.SP--;
        }

        /// <summary>
        /// Pops a byte from the stack.
        /// </summary>
        /// <returns>A byte.</returns>
        public byte PopByte()
        {
            registers.SP++;
            return Read((ushort)(0x0100 + registers.SP));
        }

        /// <summary>
        /// Pops a word from the stack.
        /// </summary>
        /// <returns>An ushort.</returns>
        public ushort PopWord()
        {
            ushort word = PopByte();
            word |= (ushort)(PopByte() << 8);
            return word;
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
            }
            else if (!(CurrentInstruction.AddressingMode == "Implied"))
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
            Log.Debug(subsystem, "Resetting CPU to power-up state.");
            registers.A = 0;
            registers.X = 0;
            registers.Y = 0;
            registers.SP = 0xFF;
            registers.PC = ReadWord(0xFFFC); // Read the PC from the reset vector.
            registers.P = (byte)(CPUFlags.None | CPUFlags.Unused | CPUFlags.InterruptDisable);
            cycles = 8;
            Log.Debug(subsystem, "CPU reset complete.");

            // assemble a string representing the current register values (except the status register)
            string registersString = string.Format("A:{0:X2} X:{1:X2} Y:{2:X2} SP:{3:X2} PC:{4:X4}", registers.A, registers.X, registers.Y, registers.SP, registers.PC);
            // now assemble a string representing the current status register values as individual bits
			string statusString = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                registers.GetFlag(CPUFlags.Negative) ? 'N' : 'n',
	            registers.GetFlag(CPUFlags.Overflow) ? 'V' : 'v',
	            registers.GetFlag(CPUFlags.Unused) ? 'U' : 'u',
	            registers.GetFlag(CPUFlags.Break) ? 'B' : 'b',
	            registers.GetFlag(CPUFlags.Decimal) ? 'D' : 'd',
	            registers.GetFlag(CPUFlags.InterruptDisable) ? 'I' : 'i',
	            registers.GetFlag(CPUFlags.Zero) ? 'Z' : 'z',
	            registers.GetFlag(CPUFlags.Carry) ? 'C' : 'c');

            // Assemble the final string, with the register values and status register bits on separate lines
            string finalString = string.Format("{0} {1}", registersString, statusString);

            Log.Debug(subsystem, finalString);
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
        /// Disassembles a range of addresses.
        /// </summary>
        /// <param name="startAddress">The address to begin disassembling from.</param>
        /// <param name="count">The number of addresses to disassemble</param>
        /// <returns>An array of string.</returns>
        public string[] Disassemble(ushort startAddress, int count)
        {
            /*
             * We need to disassemble the instructions in the range of addresses specified by startAddress and count.
             * 
             * We'll do this by creating a list of strings, and adding each disassembled instruction to the list. We
             * need to keep track of the current address, and increment it by the number of bytes in the instruction,
             * as well as not trying to disassemble operands.
             * 
             * Non-instruction bytes will return a string containing the word "DATA"
             */
            ushort address = startAddress;
            Instruction? previousInstruction = null;
            List<string> instructions = new();

            // Loop through the addresses.
            for (int i = 0; i < count; i++)
            {
                // Check if we're disassembling an operand.
                if (previousInstruction != null && previousInstruction.AddressingMode == "Immediate")
                {
                    // We're disassembling an operand, so we just add "DATA" to the list.
                    instructions.Add($"DATA");
                }

                // Read the instruction at the current address.
                byte opcode = Read(address);

                // Get the instruction.
                Instruction instruction = InstructionSet.Decode(opcode);

                // Disassemble the instruction.
                string disassembledInstruction = Disassemble(instruction);
                instructions.Add(disassembledInstruction);

                // Go through the addressing mode and add the operand to the instruction.
                switch (instruction.AddressingMode)
                {
                    case "Immediate":
                        {
                            // Get the operand.
                            byte operand = Read((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" #{operand:X2}";
                            break;
                        }
                    case "ZeroPage":
                        {
                            // Get the operand.
                            byte operand = Read((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" ${operand:X2}";
                            break;
                        }
                    case "ZeroPageX":
                        {
                            // Get the operand.
                            byte operand = Read((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" ${operand:X2},X";
                            break;
                        }
                    case "ZeroPageY":
                        {
                            // Get the operand.
                            byte operand = Read((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" ${operand:X2},Y";
                            break;
                        }
                    case "Absolute":
                        {
                            // Get the operand.
                            ushort operand = ReadWord((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" ${operand:X4}";
                            break;
                        }
                    case "AbsoluteX":
                        {
                            // Get the operand.
                            ushort operand = ReadWord((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" ${operand:X4},X";
                            break;
                        }
                    case "AbsoluteY":
                        {
                            // Get the operand.
                            ushort operand = ReadWord((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" ${operand:X4},Y";
                            break;
                        }
                    case "Indirect":
                        {
                            // Get the operand.
                            ushort operand = ReadWord((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" (${operand:X4})";
                            break;
                        }
                    case "IndirectX":
                        {
                            // Get the operand.
                            byte operand = Read((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" (${operand:X2},X)";
                            break;
                        }
                    case "IndirectY":
                        {
                            // Get the operand.
                            byte operand = Read((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" (${operand:X2}),Y";
                            break;
                        }
                    case "Relative":
                        {
                            // Get the operand.
                            byte operand = Read((ushort)(address + 1));

                            // Add the operand to the instruction.
                            instructions[^1] += $" ${operand:X2}";
                            break;
                        }
                }

                // Increment the address by the number of bytes in the instruction.
                address += (ushort)instruction.Length;

                // Store the instruction and address.
                previousInstruction = instruction;
            }

            // Return the list of disassembled instructions.
            return instructions.ToArray();
        }

        /// <summary>
        /// Disassembles an instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <returns>The disassembled instruction.</returns>
        public string Disassemble(Instruction instruction)
        {
            // Get the instruction's address mode.
            string addressingMode = instruction.AddressingMode;

            ushort operand;
            string operandString = string.Empty;

            // Get the string representation of the operand.
            switch (addressingMode)
            {
                case "Immediate":
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"#${operand:X2}";
                    break;
                case "ZeroPage":
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"${operand:X2}";
                    break;
                case "ZeroPageX":
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"${operand:X2},X";
                    break;
                case "ZeroPageY":
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"${operand:X2},Y";
                    break;
                case "Absolute":
                    operand = ReadWord((ushort)(registers.PC + 1));
                    operandString = $"${operand:X4}";
                    break;
                case "AbsoluteX":
                    operand = ReadWord((ushort)(registers.PC + 1));
                    operandString = $"${operand:X4},X";
                    break;
                case "AbsoluteY":
                    operand = ReadWord((ushort)(registers.PC + 1));
                    operandString = $"${operand:X4},Y";
                    break;
                case "Indirect":
                    operand = ReadWord((ushort)(registers.PC + 1));
                    operandString = $"(${operand:X4})";
                    break;
                case "IndirectX":
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"(${operand:X2},X)";
                    break;
                case "IndirectY":
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"(${operand:X2}),Y";
                    break;
                case "Relative":
                    operand = Read((ushort)(registers.PC + 1));
                    operandString = $"${operand:X2}";
                    break;
            }

            // Return the disassembled instruction.
            return $"{instruction.Name} {operandString}";
        }
    }
}