﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp6502
{
    /// <summary>
    /// An instruction definition.
    /// </summary>
    public class Instruction
    {
        /// <summary>
        /// The name (mnemonic) of the instruction.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The opcode of the instruction.
        /// </summary>
        public byte Opcode { get; set; }

        /// <summary>
        /// the length of the instruction in bytes.
        /// </summary>
        public uint Length { get; set; }

        /// <summary>
        /// The number of cycles the instruction takes to execute.
        /// </summary>
        public uint Cycles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instruction can take extra cycles.
        /// </summary>
        /// <remarks>
        /// This is used for instructions that can take extra cycles when a page boundary is crossed.
        /// </remarks>
        public bool CanTakeExtraCycles { get; set; }

        /// <summary>
        /// The addressing mode of the instruction.
        /// </summary>
        public Addressing AddressingMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Instruction"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="opcode">The opcode.</param>
        /// <param name="length">The length.</param>
        /// <param name="cycles">The cycles.</param>
        /// <param name="canTakeExtraCycles">If true, can take extra cycles.</param>
        /// <param name="addressingMode">The addressing mode.</param>
        public Instruction(string name, byte opcode, uint length, uint cycles, bool canTakeExtraCycles, Addressing addressingMode)
        {
            Name = name;
            Opcode = opcode;
            Length = length;
            Cycles = cycles;
            CanTakeExtraCycles = canTakeExtraCycles;
            AddressingMode = addressingMode;
        }
    }
}