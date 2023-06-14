namespace Sharp6502
{
    /// <summary>
    /// The addressing modes.
    /// </summary>
    public enum Addressing
    {
        Implied,
        Accumulator,
        Immediate,
        ZeroPage,
        ZeroPageX,
        ZeroPageY,
        Relative,
        Absolute,
        AbsoluteX,
        AbsoluteY,
        Indirect,
        IndirectX,
        IndirectY
    }

    /// <summary>
    /// Addressing mode functions.
    /// </summary>
    public static class AddressingModes
    {
        /// <summary>
        /// Gets the address of an instruction.
        /// </summary>
        /// <param name="cpu">The CPU object</param>
        /// <param name="addressingMode">The addressing mode.</param>
        /// <returns>A byte.</returns>
        public static byte GetAddress(CPU cpu, Addressing addressingMode)
        {
            return addressingMode switch
            {
                Addressing.Implied => Implied(cpu),
                Addressing.Accumulator => Accumulator(cpu),
                Addressing.Immediate => Immediate(cpu),
                Addressing.ZeroPage => ZeroPage(cpu),
                Addressing.ZeroPageX => ZeroPageX(cpu),
                Addressing.ZeroPageY => ZeroPageY(cpu),
                Addressing.Relative => Relative(cpu),
                Addressing.Absolute => Absolute(cpu),
                Addressing.AbsoluteX => AbsoluteX(cpu),
                Addressing.AbsoluteY => AbsoluteY(cpu),
                Addressing.Indirect => Indirect(cpu),
                Addressing.IndirectX => IndirectX(cpu),
                Addressing.IndirectY => IndirectY(cpu),
                _ => throw new Exception("Invalid addressing mode."),
            };
        }

        /// <summary>
        /// The implied addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte Implied(CPU cpu)
        {
            // Set the fetched byte to the value of the accumulator
            cpu.fetchedByte = cpu.registers.A;

            // This mode never uses an extra cycle
            return 0;
        }

        /// <summary>
        /// The accumulator addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte Accumulator(CPU cpu)
        {
            // This mode never uses an extra cycle,
            // and doesn't need to fetch any data.
            return 0;
        }

        /// <summary>
        /// The immediate addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte Immediate(CPU cpu)
        {
            // Set the absolute address to the program counter
            cpu.addressAbsolute = cpu.registers.PC++;

            // This mode never uses an extra cycle
            return 0;
        }

        /// <summary>
        /// The zero page addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte ZeroPage(CPU cpu)
        {
            // Set the absolute address to the data at the program counter
            // and increment the program counter
            cpu.addressAbsolute = cpu.memory.Read(cpu.registers.PC++);
            
            // Increment the program counter
            cpu.registers.PC++;

            // The zero page addressing mode only uses the first byte of the
            // address, so we need to mask the address to 0x00FF to get the
            // correct address.
            cpu.addressAbsolute &= 0x00FF;

            // This mode never uses an extra cycle
            return 0;
        }

        /// <summary>
        /// The zero page X addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte ZeroPageX(CPU cpu)
        {
            // Set the absolute address to the data at the program counter + X
            cpu.addressAbsolute = (ushort)(cpu.memory.Read(cpu.registers.PC++) + cpu.registers.X);

            // Increment the program counter
            cpu.registers.PC++;

            // The zero page addressing mode only uses the first byte of the
            // address, so we need to mask the address to 0x00FF to get the
            // correct address.
            cpu.addressAbsolute &= 0x00FF;
            return 0;
        }

        /// <summary>
        /// The zero page Y addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte ZeroPageY(CPU cpu)
        {
            // Set the absolute address to the data at the program counter + Y
            cpu.addressAbsolute = (ushort)(cpu.memory.Read(cpu.registers.PC++) + cpu.registers.Y);

            // Increment the program counter
            cpu.registers.PC++;

            // The zero page addressing mode only uses the first byte of the
            // address, so we need to mask the address to 0x00FF to get the
            // correct address.
            cpu.addressAbsolute &= 0x00FF;
            return 0;
        }

        /// <summary>
        /// The relative addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte Relative(CPU cpu)
        {
            // Set the relative address to the data at the program counter
            cpu.addressRelative = cpu.memory.Read(cpu.registers.PC++);

            // Increment the program counter
            cpu.registers.PC++;

            // If the relative address is negative, we need to sign extend it
            // to get the correct address.
            if ((cpu.addressRelative & 0x80) != 0)
            {
                cpu.addressRelative |= 0xFF00;
            }

            // This mode never uses an extra cycle
            return 0;
        }

        /// <summary>
        /// The absolute addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte Absolute(CPU cpu)
        {
            // Get the low byte of the address, and increment the program counter
            ushort lo = cpu.memory.Read(cpu.registers.PC++);

            // Get the high byte of the address
            ushort hi = cpu.memory.Read(cpu.registers.PC++);

            // Set the absolute address to the high byte shifted left 8 bits OR'd with the low byte
            cpu.addressAbsolute = (ushort)((hi << 8) | lo);

            // This mode never uses an extra cycle
            return 0;
        }

        /// <summary>
        /// The absolute X addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte AbsoluteX(CPU cpu)
        {
            // Get the low byte of the address, and increment the program counter
            ushort lo = cpu.memory.Read(cpu.registers.PC++);

            // Get the high byte of the address
            ushort hi = cpu.memory.Read(cpu.registers.PC++);

            // Set the absolute address to the high byte shifted left 8 bits OR'd with the low byte
            cpu.addressAbsolute = (ushort)((hi << 8) | lo);
            // Add the X register to the absolute address
            cpu.addressAbsolute += cpu.registers.X;

            // If the absolute address crosses a page boundary, we need to add an extra cycle
            if ((cpu.addressAbsolute & 0xFF00) != (hi << 8))
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// The absolute Y addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte AbsoluteY(CPU cpu)
        {
            // Get the low byte of the address, and increment the program counter
            ushort lo = cpu.memory.Read(cpu.registers.PC++);

            // Get the high byte of the address
            ushort hi = cpu.memory.Read(cpu.registers.PC++);

            // Set the absolute address to the high byte shifted left 8 bits OR'd with the low byte
            cpu.addressAbsolute = (ushort)((hi << 8) | lo);
            // Add the Y register to the absolute address
            cpu.addressAbsolute += cpu.registers.Y;

            // If the absolute address crosses a page boundary, we need to add an extra cycle
            if ((cpu.addressAbsolute & 0xFF00) != (hi << 8))
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// The indirect addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte Indirect(CPU cpu)
        {
            // Get the low byte of the address, and increment the program counter
            ushort ptrLo = cpu.memory.Read(cpu.registers.PC++);
            // Get the high byte of the address, and increment the program counter
            ushort ptrHi = cpu.memory.Read(cpu.registers.PC++);

            // Set the pointer address to the high byte shifted left 8 bits OR'd with the low byte
            ushort ptr = (ushort)((ptrHi << 8) | ptrLo);

            // If the low byte of the pointer is 0xFF, we need to simulate a bug in the 6502
            // where the high byte of the address is fetched from the low byte of the pointer
            // and the high byte of the address is fetched from the low byte of the pointer + 1.
            // Otherwise, we just get the high byte of the address from the pointer + 1.
            if (ptrLo == 0x00FF)
            {
                cpu.addressAbsolute = (ushort)((cpu.memory.Read((ushort)(ptr & 0xFF00)) << 8) | cpu.memory.Read((ushort)(ptr + 0)));
            } else
            {
                cpu.addressAbsolute = (ushort)((cpu.memory.Read((ushort)(ptr + 1)) << 8) | cpu.memory.Read((ushort)(ptr + 0)));
            }

            // This mode never uses an extra cycle
            return 0;

        }

        /// <summary>
        /// The indirect X addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte IndirectX(CPU cpu)
        {
            // Store the address of the pointer and increment the program counter
            ushort ptr = cpu.memory.Read(cpu.registers.PC++);

            // Get the low byte of the address
            ushort lo = cpu.memory.Read((ushort)((ptr + cpu.registers.X) & 0x00FF));
            // Get the high byte of the address
            ushort hi = cpu.memory.Read((ushort)((ptr + cpu.registers.X + 1) & 0x00FF));

            // Set the indirect address to the high byte shifted left 8 bits OR'd with the low byte
            cpu.addressAbsolute = (ushort)((hi << 8) | lo);

            // This mode never uses an extra cycle
            return 0;
        }

        /// <summary>
        /// The indirect y addressing mode.
        /// </summary>
        /// <returns>1 if an extra cycle was used, 0 otherwise</returns>
        public static byte IndirectY(CPU cpu)
        {
            // Store the address of the pointer and increment the program counter
            ushort ptr = cpu.memory.Read(cpu.registers.PC++);

            // Get the low byte of the address
            ushort lo = cpu.memory.Read((ushort)(ptr & 0x00FF));
            // Get the high byte of the address
            ushort hi = cpu.memory.Read((ushort)((ptr + 1) & 0x00FF));

            // Set the indirect address to the high byte shifted left 8 bits OR'd with the low byte
            cpu.addressAbsolute = (ushort)((hi << 8) | lo);
            // Add the Y register to the indirect address
            cpu.addressAbsolute += cpu.registers.Y;

            // If the indirect address crosses a page boundary, we need to add an extra cycle
            if ((cpu.addressAbsolute & 0xFF00) != (hi << 8))
            {
                return 1;
            }

            return 0;
        }
    }
}
