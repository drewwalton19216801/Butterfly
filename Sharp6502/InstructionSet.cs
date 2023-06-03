﻿namespace Sharp6502
{
    /// <summary>
    /// The instruction set.
    /// </summary>
    public static class InstructionSet
    {
        public static readonly Instruction[] Instructions =
        {
            // ADC (Add with Carry)
            new Instruction(name: "ADC", opcode: 0x69, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "ADC", opcode: 0x65, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "ADC", opcode: 0x75, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "ADC", opcode: 0x6D, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "ADC", opcode: 0x7D, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteX),
            new Instruction(name: "ADC", opcode: 0x79, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteY),
            new Instruction(name: "ADC", opcode: 0x61, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.IndirectX),
            new Instruction(name: "ADC", opcode: 0x71, length: 2, cycles: 5, canTakeExtraCycles: true, addressingMode: Addressing.IndirectY),

            // AND (logical AND)
            new Instruction(name: "AND", opcode: 0x29, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "AND", opcode: 0x25, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "AND", opcode: 0x35, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "AND", opcode: 0x2D, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "AND", opcode: 0x3D, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteX),
            new Instruction(name: "AND", opcode: 0x39, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteY),
            new Instruction(name: "AND", opcode: 0x21, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.IndirectX),
            new Instruction(name: "AND", opcode: 0x31, length: 2, cycles: 5, canTakeExtraCycles: true, addressingMode: Addressing.IndirectY),

            // ASL (arithmetic shift left)
            new Instruction(name: "ASL", opcode: 0x0A, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Accumulator),
            new Instruction(name: "ASL", opcode: 0x06, length: 2, cycles: 5, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "ASL", opcode: 0x16, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "ASL", opcode: 0x0E, length: 3, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "ASL", opcode: 0x1E, length: 3, cycles: 7, canTakeExtraCycles: false, addressingMode: Addressing.AbsoluteX),

            // BCC (branch if carry clear)
            new Instruction(name: "BCC", opcode: 0x90, length: 2, cycles: 2, canTakeExtraCycles: true, addressingMode: Addressing.Relative),

            // BCS (branch if carry set)
            new Instruction(name: "BCS", opcode: 0xB0, length: 2, cycles: 2, canTakeExtraCycles: true, addressingMode: Addressing.Relative),

            // BEQ (branch if equal)
            new Instruction(name: "BEQ", opcode: 0xF0, length: 2, cycles: 2, canTakeExtraCycles: true, addressingMode: Addressing.Relative),

            // BIT (bit test)
            new Instruction(name: "BIT", opcode: 0x24, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "BIT", opcode: 0x2C, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),

            // BMI (branch if minus)
            new Instruction(name: "BMI", opcode: 0x30, length: 2, cycles: 2, canTakeExtraCycles: true, addressingMode: Addressing.Relative),

            // BNE (branch if not equal)
            new Instruction(name: "BNE", opcode: 0xD0, length: 2, cycles: 2, canTakeExtraCycles: true, addressingMode: Addressing.Relative),

            // BPL (branch if positive)
            new Instruction(name: "BPL", opcode: 0x10, length: 2, cycles: 2, canTakeExtraCycles: true, addressingMode: Addressing.Relative),

            // BRK (force interrupt)
            new Instruction(name: "BRK", opcode: 0x00, length: 1, cycles: 7, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // BVC (branch if overflow clear)
            new Instruction(name: "BVC", opcode: 0x50, length: 2, cycles: 2, canTakeExtraCycles: true, addressingMode: Addressing.Relative),

            // BVS (branch if overflow set)
            new Instruction(name: "BVS", opcode: 0x70, length: 2, cycles: 2, canTakeExtraCycles: true, addressingMode: Addressing.Relative),

            // CLC (clear carry flag)
            new Instruction(name: "CLC", opcode: 0x18, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // CLD (clear decimal mode)
            new Instruction(name: "CLD", opcode: 0xD8, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // CLI (clear interrupt disable)
            new Instruction(name: "CLI", opcode: 0x58, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // CLV (clear overflow flag)
            new Instruction(name: "CLV", opcode: 0xB8, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // CMP (compare)
            new Instruction(name: "CMP", opcode: 0xC9, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "CMP", opcode: 0xC5, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "CMP", opcode: 0xD5, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "CMP", opcode: 0xCD, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "CMP", opcode: 0xDD, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteX),
            new Instruction(name: "CMP", opcode: 0xD9, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteY),
            new Instruction(name: "CMP", opcode: 0xC1, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.IndirectX),
            new Instruction(name: "CMP", opcode: 0xD1, length: 2, cycles: 5, canTakeExtraCycles: true, addressingMode: Addressing.IndirectY),

            // CPX (compare X register)
            new Instruction(name: "CPX", opcode: 0xE0, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "CPX", opcode: 0xE4, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "CPX", opcode: 0xEC, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),

            // CPY (compare Y register)
            new Instruction(name: "CPY", opcode: 0xC0, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "CPY", opcode: 0xC4, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "CPY", opcode: 0xCC, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),

            // DEC (decrement memory)
            new Instruction(name: "DEC", opcode: 0xC6, length: 2, cycles: 5, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "DEC", opcode: 0xD6, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "DEC", opcode: 0xCE, length: 3, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "DEC", opcode: 0xDE, length: 3, cycles: 7, canTakeExtraCycles: false, addressingMode: Addressing.AbsoluteX),

            // DEX (decrement X)
            new Instruction(name: "DEX", opcode: 0xCA, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // DEY (decrement Y)
            new Instruction(name: "DEY", opcode: 0x88, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // EOR (exclusive or)
            new Instruction(name: "EOR", opcode: 0x49, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "EOR", opcode: 0x45, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "EOR", opcode: 0x55, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "EOR", opcode: 0x4D, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "EOR", opcode: 0x5D, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteX),
            new Instruction(name: "EOR", opcode: 0x59, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteY),
            new Instruction(name: "EOR", opcode: 0x41, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.IndirectX),
            new Instruction(name: "EOR", opcode: 0x51, length: 2, cycles: 5, canTakeExtraCycles: true, addressingMode: Addressing.IndirectY),

            // INC (increment memory)
            new Instruction(name: "INC", opcode: 0xE6, length: 2, cycles: 5, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "INC", opcode: 0xF6, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "INC", opcode: 0xEE, length: 3, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "INC", opcode: 0xFE, length: 3, cycles: 7, canTakeExtraCycles: false, addressingMode: Addressing.AbsoluteX),

            // INX (increment X)
            new Instruction(name: "INX", opcode: 0xE8, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // INY (increment Y)
            new Instruction(name: "INY", opcode: 0xC8, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // JMP (jump)
            new Instruction(name: "JMP", opcode: 0x4C, length: 3, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "JMP", opcode: 0x6C, length: 3, cycles: 5, canTakeExtraCycles: false, addressingMode: Addressing.Indirect),

            // JSR (jump subroutine)
            new Instruction(name: "JSR", opcode: 0x20, length: 3, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),

            // LDA (load accumulator)
            new Instruction(name: "LDA", opcode: 0xA9, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "LDA", opcode: 0xA5, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "LDA", opcode: 0xB5, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "LDA", opcode: 0xAD, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "LDA", opcode: 0xBD, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteX),
            new Instruction(name: "LDA", opcode: 0xB9, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteY),
            new Instruction(name: "LDA", opcode: 0xA1, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.IndirectX),
            new Instruction(name: "LDA", opcode: 0xB1, length: 2, cycles: 5, canTakeExtraCycles: true, addressingMode: Addressing.IndirectY),

            // LDX (load X)
            new Instruction(name: "LDX", opcode: 0xA2, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "LDX", opcode: 0xA6, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "LDX", opcode: 0xB6, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageY),
            new Instruction(name: "LDX", opcode: 0xAE, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "LDX", opcode: 0xBE, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteY),

            // LDY (load Y)
            new Instruction(name: "LDY", opcode: 0xA0, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "LDY", opcode: 0xA4, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "LDY", opcode: 0xB4, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "LDY", opcode: 0xAC, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "LDY", opcode: 0xBC, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteX),

            // LSR (logical shift right)
            new Instruction(name: "LSR", opcode: 0x4A, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Accumulator),
            new Instruction(name: "LSR", opcode: 0x46, length: 2, cycles: 5, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "LSR", opcode: 0x56, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "LSR", opcode: 0x4E, length: 3, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "LSR", opcode: 0x5E, length: 3, cycles: 7, canTakeExtraCycles: false, addressingMode: Addressing.AbsoluteX),

            // NOP (no operation)
            new Instruction(name: "NOP", opcode: 0xEA, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // ORA (logical inclusive or)
            new Instruction(name: "ORA", opcode: 0x09, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "ORA", opcode: 0x05, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "ORA", opcode: 0x15, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "ORA", opcode: 0x0D, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "ORA", opcode: 0x1D, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteX),
            new Instruction(name: "ORA", opcode: 0x19, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteY),
            new Instruction(name: "ORA", opcode: 0x01, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.IndirectX),
            new Instruction(name: "ORA", opcode: 0x11, length: 2, cycles: 5, canTakeExtraCycles: true, addressingMode: Addressing.IndirectY),

            // PHA (push accumulator)
            new Instruction(name: "PHA", opcode: 0x48, length: 1, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // PHP (push processor status)
            new Instruction(name: "PHP", opcode: 0x08, length: 1, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // PLA (pull accumulator)
            new Instruction(name: "PLA", opcode: 0x68, length: 1, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // PLP (pull processor status)
            new Instruction(name: "PLP", opcode: 0x28, length: 1, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // ROL (rotate left)
            new Instruction(name: "ROL", opcode: 0x2A, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Accumulator),
            new Instruction(name: "ROL", opcode: 0x26, length: 2, cycles: 5, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "ROL", opcode: 0x36, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "ROL", opcode: 0x2E, length: 3, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "ROL", opcode: 0x3E, length: 3, cycles: 7, canTakeExtraCycles: false, addressingMode: Addressing.AbsoluteX),

            // ROR (rotate right)
            new Instruction(name: "ROR", opcode: 0x6A, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Accumulator),
            new Instruction(name: "ROR", opcode: 0x66, length: 2, cycles: 5, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "ROR", opcode: 0x76, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "ROR", opcode: 0x6E, length: 3, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "ROR", opcode: 0x7E, length: 3, cycles: 7, canTakeExtraCycles: false, addressingMode: Addressing.AbsoluteX),

            // RTI (return from interrupt)
            new Instruction(name: "RTI", opcode: 0x40, length: 1, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // RTS (return from subroutine)
            new Instruction(name: "RTS", opcode: 0x60, length: 1, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // SBC (subtract with carry)
            new Instruction(name: "SBC", opcode: 0xE9, length: 2, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Immediate),
            new Instruction(name: "SBC", opcode: 0xE5, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "SBC", opcode: 0xF5, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "SBC", opcode: 0xED, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "SBC", opcode: 0xFD, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteX),
            new Instruction(name: "SBC", opcode: 0xF9, length: 3, cycles: 4, canTakeExtraCycles: true, addressingMode: Addressing.AbsoluteY),
            new Instruction(name: "SBC", opcode: 0xE1, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.IndirectX),
            new Instruction(name: "SBC", opcode: 0xF1, length: 2, cycles: 5, canTakeExtraCycles: true, addressingMode: Addressing.IndirectY),

            // SEC (set carry flag)
            new Instruction(name: "SEC", opcode: 0x38, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // SED (set decimal flag)
            new Instruction(name: "SED", opcode: 0xF8, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // SEI (set interrupt disable flag)
            new Instruction(name: "SEI", opcode: 0x78, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // STA (store accumulator)
            new Instruction(name: "STA", opcode: 0x85, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "STA", opcode: 0x95, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "STA", opcode: 0x8D, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),
            new Instruction(name: "STA", opcode: 0x9D, length: 3, cycles: 5, canTakeExtraCycles: false, addressingMode: Addressing.AbsoluteX),
            new Instruction(name: "STA", opcode: 0x99, length: 3, cycles: 5, canTakeExtraCycles: false, addressingMode: Addressing.AbsoluteY),
            new Instruction(name: "STA", opcode: 0x81, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.IndirectX),
            new Instruction(name: "STA", opcode: 0x91, length: 2, cycles: 6, canTakeExtraCycles: false, addressingMode: Addressing.IndirectY),

            // STX (store X)
            new Instruction(name: "STX", opcode: 0x86, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "STX", opcode: 0x96, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageY),
            new Instruction(name: "STX", opcode: 0x8E, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),

            // STY (store Y)
            new Instruction(name: "STY", opcode: 0x84, length: 2, cycles: 3, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPage),
            new Instruction(name: "STY", opcode: 0x94, length: 2, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.ZeroPageX),
            new Instruction(name: "STY", opcode: 0x8C, length: 3, cycles: 4, canTakeExtraCycles: false, addressingMode: Addressing.Absolute),

            // TAX (transfer accumulator to X)
            new Instruction(name: "TAX", opcode: 0xAA, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // TAY (transfer accumulator to Y)
            new Instruction(name: "TAY", opcode: 0xA8, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // TSX (transfer stack pointer to X)
            new Instruction(name: "TSX", opcode: 0xBA, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // TXA (transfer X to accumulator)
            new Instruction(name: "TXA", opcode: 0x8A, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // TXS (transfer X to stack pointer)
            new Instruction(name: "TXS", opcode: 0x9A, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),

            // TYA (transfer Y to accumulator)
            new Instruction(name: "TYA", opcode: 0x98, length: 1, cycles: 2, canTakeExtraCycles: false, addressingMode: Addressing.Implied),
        };

        /// <summary>
        /// Decodes an opcode into an Instruction.
        /// </summary>
        /// <param name="opcode">The opcode.</param>
        /// <returns>The Instruction.</returns>
        public static Instruction Decode(byte opcode)
        {
            foreach(Instruction instruction in Instructions)
            {
                if (instruction.Opcode == opcode)
                {
                    return instruction;
                }
            }

            // If we get here, we have an invalid opcode
            return new Instruction(name: "???", opcode: 0xFF, length: 1, cycles: 1, canTakeExtraCycles: false, addressingMode: Addressing.Implied);
        }
    }
}
