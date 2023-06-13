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
            string instructionName = cpu.CurrentInstruction?.Name ?? "???";

            // Get the method
            MethodInfo? method = typeof(InstructionExecutor).GetMethod(instructionName, BindingFlags.Public | BindingFlags.Static);

            if (method == null)
            {
                // The method doesn't exist, so we'll throw an exception
                throw new InvalidOperationException($"The instruction \"{instructionName}\" does not exist.");
            }

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
            // TODO: Implement decimal mode

            // Fetch the data to add
            cpu.Fetch();

            // We need perform the addition in 16-bit mode so we can detect the carry bit
            ushort result = (ushort)(cpu.registers.A + cpu.fetchedByte + (cpu.registers.GetFlag(CPUFlags.Carry) ? 1 : 0));

            // Set the carry flag if the result is greater than 255
            cpu.registers.SetFlag(CPUFlags.Carry, result > 255);

            // Set the zero flag if the result is 0
            cpu.registers.SetFlag(CPUFlags.Zero, (result & 0x00FF) == 0);

            // Set the overflow flag if the result is greater than 127 or less than -128
            cpu.registers.SetFlag(CPUFlags.Overflow, ((cpu.registers.A ^ result) & (cpu.fetchedByte ^ result) & 0x80) != 0);

            // Set the negative flag if the result is less than 0
            cpu.registers.SetFlag(CPUFlags.Negative, (result & 0x80) != 0);

            // Store the result in the accumulator
            cpu.registers.A = (byte)(result & 0x00FF);

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
            cpu.PushStack((byte)(cpu.registers.PC >> 8 & 0x00FF));
            // Push the PC to the stack (low byte last)
            cpu.PushStack((byte)(cpu.registers.PC & 0x00FF));

            // Set the break flag
            cpu.registers.SetFlag(CPUFlags.Break, true);

            // Push the status register to the stack
            cpu.PushStack(cpu.registers.P);

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
            return 0;
        }

        /// <summary>
        /// The CLD (Clear Decimal Mode) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte CLD(CPU cpu)
        {
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
            ushort data = (ushort)(cpu.fetchedByte + 1);

            // Write the data back to memory
            cpu.memory.Write(cpu.addressAbsolute, (byte)(data & 0x00FF));

            // Set the zero flag if the data is zero
            cpu.registers.SetFlag(CPUFlags.Zero, (data & 0x00FF) == 0x0000);

            // Set the negative flag if bit 7 is set
            cpu.registers.SetFlag(CPUFlags.Negative, (data & 0x0080) > 0);

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
            cpu.PushStack((byte)((cpu.registers.PC >> 8) & 0x00FF));

            // Push the low byte of the PC to the stack
            cpu.PushStack((byte)(cpu.registers.PC & 0x00FF));

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
            // Fetch the next byte from memory
            cpu.Fetch();

            // Rotate the fetched byte right
            cpu.fetchedByte = (byte)((cpu.fetchedByte >> 1) | (cpu.registers.GetFlag(CPUFlags.Carry) ? 0x80 : 0x00));

            // Set the carry flag if the 0 bit was set
            cpu.registers.SetFlag(CPUFlags.Carry, (cpu.fetchedByte & 0x01) > 0);
            
            // Set the zero flag if the fetched byte is zero
            cpu.registers.SetFlag(CPUFlags.Zero, cpu.fetchedByte == 0);

            // Set the negative flag if the fetched byte is negative
            cpu.registers.SetFlag(CPUFlags.Negative, (cpu.fetchedByte & 0x80) > 0);

            // If the instruction is in implied mode, store the fetched byte in the accumulator
            if (cpu.CurrentInstruction?.AddressingMode == Addressing.Implied)
            {
                cpu.registers.A = cpu.fetchedByte;
            }
            else
            {
                // Store the fetched byte in memory
                cpu.Write(cpu.addressAbsolute, cpu.fetchedByte);
            }

            // Return 0 since this instruction does not use an extra cycle
            return 0;
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
            cpu.registers.P = cpu.PopStack();

            // Clear the break flag
            cpu.registers.SetFlag(CPUFlags.Break, false);

            // Clear the unused flag
            cpu.registers.SetFlag(CPUFlags.Unused, true);

            // Program counter is pulled from the stack
            cpu.registers.PC = cpu.PopStackWord();

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
            return 0;
        }

        /// <summary>
        /// The SBC (Subtract Memory from Accumulator with Borrow) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte SBC(CPU cpu)
        {
            // TODO: implement decimal mode

            // Fetch the next byte from memory
            cpu.Fetch();

            // Invert the fetched byte
            ushort value = (ushort)(cpu.fetchedByte ^ 0x00FF);

            // Now we can add with carry as normal
            ushort temp = (ushort)(cpu.registers.A + value + (cpu.registers.GetFlag(CPUFlags.Carry) ? 1 : 0));

            // Set the carry flag if the result is greater than 255
            cpu.registers.SetFlag(CPUFlags.Carry, (temp & 0xFF00) > 0);

            // Set the zero flag if the result is 0
            cpu.registers.SetFlag(CPUFlags.Zero, (temp & 0x00FF) == 0);

            // Set the overflow flag if the result is greater than 127 or less than -128
            cpu.registers.SetFlag(CPUFlags.Overflow, ((temp ^ cpu.registers.A) & (temp ^ value) & 0x0080) > 0);

            // Set the negative flag if the result is negative
            cpu.registers.SetFlag(CPUFlags.Negative, (temp & 0x0080) > 0);

            // Load the result into the accumulator
            cpu.registers.A = (byte)(temp & 0x00FF);

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
