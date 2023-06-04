namespace Sharp6502
{
    /// <summary>
    /// The instruction execution engine
    /// </summary>
    public static class InstructionExecutor
    {
        /// <summary>
        /// The ADC (Add with Carry) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte ADC(CPU cpu)
        {
            return 0;
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
            return 0;
        }

        /// <summary>
        /// The INX (Increment Index X by One) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte INX(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The INY (Increment Index Y by One) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte INY(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The JMP (Jump to New Location) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte JMP(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The JSR (Jump to New Location Saving Return Address) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte JSR(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The LDA (Load Accumulator with Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte LDA(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The LDX (Load Index X with Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte LDX(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The LDY (Load Index Y with Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte LDY(CPU cpu)
        {
            return 0;
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
            return 0;
        }

        /// <summary>
        /// The RTI (Return from Interrupt) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte RTI(CPU cpu)
        {
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
            return 0;
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
            return 0;
        }

        /// <summary>
        /// The STX (Store Index X in Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte STX(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The STY (Store Index Y in Memory) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte STY(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The TAX (Transfer Accumulator to Index X) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TAX(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The TAY (Transfer Accumulator to Index Y) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TAY(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The TSX (Transfer Stack Pointer to Index X) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TSX(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The TXA (Transfer Index X to Accumulator) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TXA(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The TXS (Transfer Index X to Stack Pointer) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TXS(CPU cpu)
        {
            return 0;
        }

        /// <summary>
        /// The TYA (Transfer Index Y to Accumulator) instruction.
        /// </summary>
        /// <param name="cpu">CPU object</param>
        /// <returns>1 if the instruction used an extra cycle, otherwise 0</returns>
        public static byte TYA(CPU cpu)
        {
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
