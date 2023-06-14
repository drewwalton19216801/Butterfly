using System.Reflection;

namespace Sharp6502
{
    /// <summary>
    /// The instruction execution engine
    /// </summary>
    public static class InstructionExecutor
    {
        /// <summary>
        /// Executes the instruction.
        /// </summary>
        /// <param name="cpu">The CPU object</param>
        /// <returns>1 if an additional cycle was used, otherwise 0</returns>
        public static byte ExecuteInstruction(CPU cpu)
        {
            // This is extremely hacky, but it works
            // --------------------------------------
            // We're going to use reflection to get the method that corresponds to the instruction name
            // and then invoke it. This is a lot faster than a switch statement, and it's a lot easier
            // to maintain.
            // --------------------------------------

            // Get the instruction name
            string instructionName = cpu.CurrentInstruction?.Name ?? "XXX";

            // Get the method
            MethodInfo? method = typeof(InstructionExecutor).GetMethod(instructionName, BindingFlags.Public | BindingFlags.Static) ?? throw new InvalidOperationException($"The instruction \"{instructionName}\" does not exist.");

            // Invoke the method
            object result = (byte)method.Invoke(null, new object[] { cpu });

            // Handle the result
            if (result is byte byteResult)
            {
                return byteResult;
            }
            else
            {
                throw new InvalidOperationException($"The instruction \"{instructionName}\" returned an invalid result.");
            }
        }

        /// <summary>
        /// The ADC (Add with Carry) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte ADC(CPU cpu)
        {
            // Fetch the data to add
            cpu.Fetch();

            // Add the value to the accumulator
            cpu.temp = (ushort)(cpu.registers.A + cpu.fetchedByte + (cpu.registers.GetFlag(CPUFlags.Carry) ? 1 : 0));

            // Set the zero flag if the result is 0
            cpu.registers.SetFlag(CPUFlags.Zero, (cpu.temp & 0x00FF) == 0);

            // If the CPU variant is NOT NES_6502, then we check for decimal mode
            if (cpu.cpuVariant != CPU.Variant.NES_6502)
            {
                // If the decimal flag is set, then we need to convert the result to BCD
                if (cpu.registers.GetFlag(CPUFlags.Decimal))
                {
                    // If the result is greater than 99, then we need to add 96 to the result
                    if (((cpu.registers.A & 0xF) + (cpu.fetchedByte & 0xF) + (cpu.registers.GetFlag(CPUFlags.Carry) ? 1 : 0)) > 9)
                    {
                        cpu.temp += 6;
                    }

                    // Set the negative flag if the result is negative
                    cpu.registers.SetFlag(CPUFlags.Negative, (cpu.temp & 0x80) != 0);

                    // Set the overflow flag if the result is greater than 127 or less than -128
                    cpu.registers.SetFlag(CPUFlags.Overflow, ((cpu.registers.A ^ cpu.temp) & (cpu.fetchedByte ^ cpu.temp) & 0x80) != 0);

                    // If the result is greater than 99, then we need to add 96 to the result
                    if (cpu.temp > 99)
                    {
                        cpu.temp += 96;
                    }

                    // Set the carry flag if the result is greater than 0x99
                    cpu.registers.SetFlag(CPUFlags.Carry, cpu.temp > 0x99);
                }
            } else
            {
                // Set the negative flag if the result is less than 0
                cpu.registers.SetFlag(CPUFlags.Negative, (cpu.temp & 0x80) != 0);

                // Set the overflow flag if the result is greater than 127 or less than -128
                cpu.registers.SetFlag(CPUFlags.Overflow, ((cpu.registers.A ^ cpu.temp) & (cpu.fetchedByte ^ cpu.temp) & 0x80) != 0);

                // Set the carry flag to the opposite of (cpu.temp > 0xFF)
                cpu.registers.SetFlag(CPUFlags.Carry, cpu.temp > 0xFF);
            }

            // Store the result in the accumulator
            cpu.registers.A = (byte)(cpu.temp & 0x00FF);

            // This instruction can take an extra cycle
            return 1;
        }

        /// <summary>
        /// The AND (AND Memory with Accumulator) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte AND(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The ASL (Arithmetic Shift Left) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte ASL(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The BCC (Branch if Carry Clear) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BCC(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The BCS (Branch if Carry Set) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BCS(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The BEQ (Branch if Equal to Zero) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BEQ(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The BIT (Test Bits in Memory with Accumulator) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BIT(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The BMI (Branch if Minus) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BMI(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The BNE (Branch if Not Equal to Zero) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BNE(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The BPL (Branch if Positive) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BPL(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The BRK (Force Break) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BRK(CPU cpu)
        {
            // Increment the PC
            cpu.registers.PC++;

            // Set the interrupt flag
            cpu.registers.SetFlag(CPUFlags.InterruptDisable, true);

            // Push the PC to the stack (high byte first)
            cpu.PushByte((byte)(cpu.registers.PC >> 8 & 0x00FF));
            // Push the PC to the stack (low byte last)
            cpu.PushByte((byte)(cpu.registers.PC & 0x00FF));

            // Set the break flag
            cpu.registers.SetFlag(CPUFlags.Break, true);

            // Push the status register to the stack
            cpu.PushByte(cpu.registers.P);

            // Clear the break flag
            cpu.registers.SetFlag(CPUFlags.Break, false);

            // Set the PC to the data at the interrupt vector
            cpu.registers.PC = (ushort)(cpu.memory.Read(0xFFFE) | cpu.memory.Read(0xFFFF) << 8);

            // Return 0 cycles
            return 0;
        }

        /// <summary>
        /// The BVC (Branch if Overflow Clear) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BVC(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The BVS (Branch if Overflow Set) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte BVS(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The CLC (Clear Carry Flag) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte CLC(CPU cpu)
        {
            // Clear the carry flag
            cpu.registers.SetFlag(CPUFlags.Carry, false);

            // Return 0 extra cycles
            return 0;
        }

        /// <summary>
        /// The CLD (Clear Decimal Mode) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte CLD(CPU cpu)
        {
            // Clear the decimal flag
            cpu.registers.SetFlag(CPUFlags.Decimal, false);
            
            // Return 0 extra cycles
            return 0;
        }

        /// <summary>
        /// The CLI (Clear Interrupt Disable Bit) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte CLI(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The CLV (Clear Overflow Flag) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte CLV(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The CMP (Compare Memory with Accumulator) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte CMP(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The CPX (Compare Memory and Index X) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte CPX(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The CPY (Compare Memory and Index Y) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte CPY(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The DEC (Decrement Memory by One) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte DEC(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The DEX (Decrement Index X by One) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte DEX(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The DEY (Decrement Index Y by One) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte DEY(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The EOR (Exclusive-OR Memory with Accumulator) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte EOR(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The INC (Increment Memory by One) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte INC(CPU cpu)
        {
            // Fetch the data
            cpu.Fetch();

            // Increment the data
            cpu.temp = (ushort)(cpu.fetchedByte + 1);

            // Write the data back to memory
            cpu.memory.Write(cpu.addressAbsolute, (byte)(cpu.temp & 0x00FF));

            // Set the zero flag if the data is zero
            cpu.registers.SetFlag(CPUFlags.Zero, (cpu.temp & 0x00FF) == 0x0000);

            // Set the negative flag if bit 7 is set
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.temp & 0x0080) > 0);

            // We didn't use an extra cycle
            return 0;
        }

        /// <summary>
        /// The INX (Increment Index X by One) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte INX(CPU cpu)
        {
            // Increment the X register
            cpu.registers.X++;

            // Set the zero flag if the X register is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.X == 0x00);

            // Set the negative flag if bit 7 is set
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.X & 0x80) > 0);

            // We didn't use an extra cycle
            return 0;
        }

        /// <summary>
        /// The INY (Increment Index Y by One) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte INY(CPU cpu)
        {
            // Increment the Y register
            cpu.registers.Y++;

            // Set the zero flag if the Y register is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.Y == 0x00);

            // Set the negative flag if bit 7 is set
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.Y & 0x80) > 0);

            // We didn't use an extra cycle
            return 0;
        }

        /// <summary>
        /// The JMP (Jump to New Location) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte JMP(CPU cpu)
        {
            // Set the PC to the absolute address of the operand
            cpu.registers.PC = cpu.addressAbsolute;

            // Return 0 because the instruction did not use an extra cycle
            return 0;
        }

        /// <summary>
        /// The JSR (Jump to New Location Saving Return Address) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte JSR(CPU cpu)
        {
            // Decrement the PC by 1
            cpu.registers.PC--;

            // Push the high byte of the PC to the stack
            cpu.PushByte((byte)((cpu.registers.PC >> 8) & 0x00FF));

            // Push the low byte of the PC to the stack
            cpu.PushByte((byte)(cpu.registers.PC & 0x00FF));

            // Set the PC to the absolute address of the operand
            cpu.registers.PC = cpu.addressAbsolute;

            // Return 0 because the instruction did not use an extra cycle
            return 0;
        }

        /// <summary>
        /// The LDA (Load Accumulator with Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte LDA(CPU cpu)
        {
            // Fetch the next byte from memory
            cpu.Fetch();

            // Load the fetched byte into the accumulator
            cpu.registers.A = cpu.fetchedByte;

            // Set the zero flag if the accumulator is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.A == 0);

            // Set the negative flag if the accumulator is negative
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.A & 0x80) > 0);

            // Return 1 since this instruction uses an extra cycle
            return 1;
        }

        /// <summary>
        /// The LDX (Load Index X with Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte LDX(CPU cpu)
        {
            // Fetch the next byte from memory
            cpu.Fetch();

            // Load the fetched byte into the X register
            cpu.registers.X = cpu.fetchedByte;

            // Set the zero flag if the X register is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.X == 0);

            // Set the negative flag if the X register is negative
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.X & 0x80) > 0);

            // Return 1 since this instruction uses an extra cycle
            return 1;
        }

        /// <summary>
        /// The LDY (Load Index Y with Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte LDY(CPU cpu)
        {
            // Fetch the next byte from memory
            cpu.Fetch();

            // Load the fetched byte into the Y register
            cpu.registers.Y = cpu.fetchedByte;

            // Set the zero flag if the Y register is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.Y == 0);

            // Set the negative flag if the Y register is negative
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.Y & 0x80) > 0);

            // Return 1 since this instruction uses an extra cycle
            return 1;
        }

        /// <summary>
        /// The LSR (Shift One Bit Right (Memory or Accumulator)) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte LSR(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The NOP (No Operation) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte NOP(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The ORA (OR Memory with Accumulator) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte ORA(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The PHA (Push Accumulator on Stack) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte PHA(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The PHP (Push Processor Status on Stack) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte PHP(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The PLA (Pull Accumulator from Stack) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte PLA(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The PLP (Pull Processor Status from Stack) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte PLP(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The ROL (Rotate One Bit Left (Memory or Accumulator)) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte ROL(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The ROR (Rotate One Bit Right (Memory or Accumulator)) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte ROR(CPU cpu)
        {
            return cpu.cpuVariant switch
            {
                CPU.Variant.CMOS_6502 => ROR_CMOS(cpu),
                CPU.Variant.NMOS_6502 => ROR_NMOS(cpu),
                _ => throw new Exception("Invalid CPU variant"),
            };
        }

        /// <summary>
        /// ROR (Rotate One Bit Right (Memory or Accumulator)) instruction for the NMOS 6502.
        /// </summary>
        /// <param name="cpu">The cpu.</param>
        /// <returns>A byte.</returns>
        /// <remarks>
        /// The NMOS 6502 has a bug in the ROR instruction. It shifts left instead of right,
        /// shifts a 0 into the 9th bit (instead of the carry flag), and does not affect the
        /// carry flag.
        /// </remarks>
        private static byte ROR_NMOS(CPU cpu)
        {
            // Are we in accumulator mode?
            if (cpu.CurrentInstruction?.Opcode == 0x6A)
            {
                // Load the accumulator into the temp variable
                cpu.temp = cpu.registers.A;

                // Set the 9th bit of the temp variable to 0
                cpu.temp &= 0x7F;

                // Shift the temp variable left by 1
                cpu.temp <<= 1;

                // Mask the temp variable to 8 bits
                cpu.temp &= 0xFF;

                // Set the negative flag if the 8th bit of the temp variable is set
                cpu.registers.SetFlag(CPUFlags.Negative, (cpu.temp & 0x80) > 0);

                // Set the zero flag if the temp variable is zero
                cpu.registers.SetFlag(CPUFlags.Zero, cpu.temp == 0);

                // Store the temp variable into the accumulator
                cpu.registers.A = (byte)cpu.temp;

                // Return 0 since this instruction does not use an extra cycle
                return 0;
            }
            else
            {
                // Fetch the next byte from memory
                cpu.Fetch();

                // Load the fetched byte into the temp variable
                cpu.temp = cpu.fetchedByte;

                // Set the 9th bit of the temp variable to 0
                cpu.temp &= 0x7F;

                // Shift the temp variable left by 1
                cpu.temp <<= 1;

                // Mask the temp variable to 8 bits
                cpu.temp &= 0xFF;

                // Set the negative flag if the 8th bit of the temp variable is set
                cpu.registers.SetFlag(CPUFlags.Negative, (cpu.temp & 0x80) > 0);

                // Set the zero flag if the temp variable is zero
                cpu.registers.SetFlag(CPUFlags.Zero, cpu.temp == 0);

                // Store the temp variable into memory
                cpu.Write(cpu.addressAbsolute, (byte)cpu.temp);

                // Return 0 since this instruction does not use an extra cycle
                return 0;
            }
        }

        /// <summary>
        /// ROR (Rotate One Bit Right (Memory or Accumulator)) instruction for the CMOS 6502.
        /// </summary>
        /// <param name="cpu">The cpu.</param>
        /// <returns>A byte.</returns>
        private static byte ROR_CMOS(CPU cpu)
        {
            // Are we in accumulator mode?
            if (cpu.CurrentInstruction?.Opcode == 0x6A)
            {
                // Load the accumulator into the temp variable
                cpu.temp = cpu.registers.A;

                // If the carry flag is set, set the 9th bit of the temp variable
                if (cpu.registers.GetFlag(CPUFlags.Carry))
                {
                    cpu.temp |= 0x100;
                }

                // Set the carry flag if the 9th bit of the temp variable is set
                cpu.registers.SetFlag(CPUFlags.Carry, (cpu.temp & 0x01) > 0);

                // Shift the temp variable right by 1
                cpu.temp >>= 1;

                // Mask the temp variable to 8 bits
                cpu.temp &= 0xFF;

                // Set the negative flag if the 8th bit of the temp variable is set
                cpu.registers.SetFlag(CPUFlags.Negative, (cpu.temp & 0x80) > 0);

                // Set the zero flag if the temp variable is zero
                cpu.registers.SetFlag(CPUFlags.Zero, cpu.temp == 0);

                // Store the temp variable into the accumulator
                cpu.registers.A = (byte)cpu.temp;

                // Return 0 since this instruction does not use an extra cycle
                return 0;
            }
            else
            {
                // Fetch the next byte from memory
                cpu.Fetch();

                // Load the fetched byte into the temp variable
                cpu.temp = cpu.fetchedByte;

                // If the carry flag is set, set the 9th bit of the temp variable
                if (cpu.registers.GetFlag(CPUFlags.Carry))
                {
                    cpu.temp |= 0x100;
                }

                // Set the carry flag if the 9th bit of the temp variable is set
                cpu.registers.SetFlag(CPUFlags.Carry, (cpu.temp & 0x01) > 0);

                // Shift the temp variable right by 1
                cpu.temp >>= 1;

                // Mask the temp variable to 8 bits
                cpu.temp &= 0xFF;

                // Set the negative flag if the 8th bit of the temp variable is set
                cpu.registers.SetFlag(CPUFlags.Negative, (cpu.temp & 0x80) > 0);

                // Set the zero flag if the temp variable is zero
                cpu.registers.SetFlag(CPUFlags.Zero, cpu.temp == 0);

                // Store the temp variable into memory
                cpu.Write(cpu.addressAbsolute, (byte)cpu.temp);

                // Return 0 since this instruction does not use an extra cycle
                return 0;
            }
        }

        /// <summary>
        /// The RTI (Return from Interrupt) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte RTI(CPU cpu)
        {
            // Increment the stack pointer
            cpu.registers.SP++;

            // Status register is pulled from the stack
            cpu.registers.P = cpu.PopByte();

            // Clear the break flag
            cpu.registers.SetFlag(CPUFlags.Break, false);

            // Clear the unused flag
            cpu.registers.SetFlag(CPUFlags.Unused, true);

            // Program counter is pulled from the stack
            cpu.registers.PC = cpu.PopWord();

            // Return 0 since this instruction does not use an extra cycle
            return 0;
        }

        /// <summary>
        /// The RTS (Return from Subroutine) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte RTS(CPU cpu)
        {
            // Pop a word from the stack and store it in the program counter
            cpu.registers.PC = cpu.PopWord();

            // Increment the program counter
            cpu.registers.PC++;

            // Return 0 since this instruction does not use an extra cycle
            return 0;
        }

        /// <summary>
        /// The SBC (Subtract Memory from Accumulator with Borrow) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte SBC(CPU cpu)
        {
            // Fetch the next byte from memory
            cpu.Fetch();

            // Perform the subtraction using two's complement
            cpu.temp = (ushort)(cpu.registers.A - cpu.fetchedByte - (cpu.registers.GetFlag(CPUFlags.Carry) ? 0 : 1));

            // Set the zero flag if the result is 0
            cpu.registers.SetFlag(CPUFlags.Zero, (cpu.temp & 0x00FF) == 0);

            // If the CPU variant is NOT NES_6502, then we check for decimal mode
            if (cpu.cpuVariant != CPU.Variant.NES_6502)
            {
                // If the decimal flag is set, then we need to convert the result to BCD
                if (cpu.registers.GetFlag(CPUFlags.Decimal))
                {
                    // Adjust the result if necessary
                    if (((cpu.registers.A & 0xF) - (cpu.fetchedByte & 0xF) - (cpu.registers.GetFlag(CPUFlags.Carry) ? 0 : 1)) < 0)
                    {
                        cpu.temp -= 6;
                    }

                    // Set the negative flag if the result is negative
                    cpu.registers.SetFlag(CPUFlags.Negative, (cpu.temp & 0x80) != 0);

                    // Set the overflow flag if the result is greater than 127 or less than -128
                    cpu.registers.SetFlag(CPUFlags.Overflow, ((cpu.registers.A ^ cpu.temp) & (~cpu.fetchedByte ^ cpu.temp) & 0x80) != 0);

                    // Adjust the result if necessary
                    if (cpu.temp > 0x99)
                    {
                        cpu.temp -= 96;
                    }

                    // Set the carry flag if the result is less than or equal to 0x99
                    cpu.registers.SetFlag(CPUFlags.Carry, cpu.temp <= 0x99);
                }
            }
            else
            {
                // Set the negative flag if the result is negative
                cpu.registers.SetFlag(CPUFlags.Negative, (cpu.temp & 0x80) != 0);

                // Set the overflow flag if the result is greater than 127 or less than -128
                cpu.registers.SetFlag(CPUFlags.Overflow, ((cpu.registers.A ^ cpu.temp) & (~cpu.fetchedByte ^ cpu.temp) & 0x80) != 0);

                // Set the carry flag to the opposite of (cpu.temp > 0xFF)
                cpu.registers.SetFlag(CPUFlags.Carry, cpu.temp <= 0xFF);
            }

            // Store the result in the accumulator
            cpu.registers.A = (byte)(cpu.temp & 0x00FF);

            // This instruction can take an extra cycle
            return 1;
        }

        /// <summary>
        /// The SEC (Set Carry Flag) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte SEC(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The SED (Set Decimal Flag) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte SED(CPU cpu)
        {
            // Set the decimal flag
            cpu.registers.SetFlag(CPUFlags.Decimal, true);

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The SEI (Set Interrupt Disable Status) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte SEI(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The STA (Store Accumulator in Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte STA(CPU cpu)
        {
            // Write the accumulator to memory
            cpu.memory.Write(cpu.addressAbsolute, cpu.registers.A);

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The STX (Store Index X in Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte STX(CPU cpu)
        {
            // Write the X register to memory
            cpu.memory.Write(cpu.addressAbsolute, cpu.registers.X);

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The STY (Store Index Y in Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte STY(CPU cpu)
        {
            // Write the Y register to memory
            cpu.memory.Write(cpu.addressAbsolute, cpu.registers.Y);

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The TAX (Transfer Accumulator to Index X) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TAX(CPU cpu)
        {
            // Copy the accumulator to the X register
            cpu.registers.X = cpu.registers.A;
            
            // Set the zero flag if the result is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.X == 0x00);

            // Set the negative flag if bit 7 is set
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.X & 0x80) > 0);

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The TAY (Transfer Accumulator to Index Y) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TAY(CPU cpu)
        {
            // Copy the accumulator to the Y register
            cpu.registers.Y = cpu.registers.A;

            // Set the zero flag if the result is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.Y == 0x00);

            // Set the negative flag if bit 7 is set
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.Y & 0x80) > 0);

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The TSX (Transfer Stack Pointer to Index X) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TSX(CPU cpu)
        {
            // Copy the stack pointer to the X register
            cpu.registers.X = cpu.registers.SP;

            // Set the zero flag if the result is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.X == 0x00);

            // Set the negative flag if bit 7 is set
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.X & 0x80) > 0);

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The TXA (Transfer Index X to Accumulator) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TXA(CPU cpu)
        {
            // Copy the X register to the accumulator
            cpu.registers.A = cpu.registers.X;

            // Set the zero flag if the result is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.A == 0x00);

            // Set the negative flag if bit 7 is set
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.A & 0x80) > 0);

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The TXS (Transfer Index X to Stack Pointer) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TXS(CPU cpu)
        {
            // Copy the X register to the stack pointer
            cpu.registers.SP = cpu.registers.X;

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The TYA (Transfer Index Y to Accumulator) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TYA(CPU cpu)
        {
            // Copy the Y register to the accumulator
            cpu.registers.A = cpu.registers.Y;

            // Set the zero flag if the result is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.registers.A == 0x00);

            // Set the negative flag if bit 7 is set
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.registers.A & 0x80) > 0);

            // No extra cycle
            return 0;
        }

        /// <summary>
        /// The XXX (Unofficial Opcode) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte XXX(CPU cpu)
        {
            return 0;
        }
    }
}
