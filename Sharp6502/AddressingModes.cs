using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// The implied addressing mode.
        /// </summary>
        /// <returns>0</returns>
        public static short Implied()
        {
            return 0; // No operand
        }

        /// <summary>
        /// The accumulator addressing mode.
        /// </summary>
        /// <returns>0</returns>
        public static short Accumulator()
        {
            return 0; // No operand
        }

        /// <summary>
        /// The immediate addressing mode.
        /// </summary>
        /// <returns>The immediate address.</returns>
        public static ushort Immediate(CPU cpu)
        {
            // Address is the PC + 1
            return cpu.registers.PC++;
        }

        /// <summary>
        /// The zero page addressing mode.
        /// </summary>
        /// <returns>The calculated zero page address.</returns>
        public static ushort ZeroPage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The zero page X addressing mode.
        /// </summary>
        /// <returns>The calculated zero page X address.</returns>
        public static ushort ZeroPageX()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The zero page Y addressing mode.
        /// </summary>
        /// <returns>The calculated zero page Y address.</returns>
        public static ushort ZeroPageY()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The relative addressing mode.
        /// </summary>
        /// <returns>The calculated relative address.</returns>
        public static ushort Relative()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The absolute addressing mode.
        /// </summary>
        /// <returns>The calculated absolute address.</returns>
        public static ushort Absolute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The absolute X addressing mode.
        /// </summary>
        /// <returns>The calculated absolute X address.</returns>
        public static ushort AbsoluteX()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The absolute Y addressing mode.
        /// </summary>
        /// <returns>The calculated absolute Y address.</returns>
        public static ushort AbsoluteY()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The indirect addressing mode.
        /// </summary>
        /// <returns>The calculated indirect address.</returns>
        public static ushort Indirect()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The indirect X addressing mode.
        /// </summary>
        /// <returns>The calculated indirect X address.</returns>
        public static ushort IndirectX()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The indirect y addressing mode.
        /// </summary>
        /// <returns>The calculated indirect Y address.</returns>
        public static ushort IndirectY()
        {
            throw new NotImplementedException();
        }
    }
}
