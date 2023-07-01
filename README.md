# Butterfly Emulator

**Butterfly** is an experiment in hardware and software design that aims to emulate a 6502-based computer system, similar in nature to systems like the Apple II and the Commodore 64. It is written in C# and is cross-platform.

# Design
**Butterfly** aims to be as extensible and customizable as possible, so that it can be modified to emulate real 6502-based computer systems.

## CPU
The CPU ([Sharp6502/CPU.cs](https://bitbucket.org/drewwalton19216801/butterfly-cs/src/dev/Sharp6502/CPU.cs)) aims to be as accurate as possible, with complete cycle-accuracy being a future goal. It intends to support multiple variants of the 6502, including the original NMOS 6502 (complete with ROR bugs!), the CMOS 65C02 and the Ricoh 2A03 (found in the NES).

### Instruction Set
The [Instruction Set](https://bitbucket.org/drewwalton19216801/butterfly-cs/src/dev/Sharp6502/InstructionSet.cs) aims to be extensible as well, allowing for the implementation of unofficial instructions, or even custom variants of the 6502 that don't have real hardware versions.

### Instruction Execution
The [Instruction Executor](https://bitbucket.org/drewwalton19216801/butterfly-cs/src/dev/Sharp6502/InstructionExecutor.cs) contains the actual code for all supported instructions. These methods are called using reflection, to make extending the CPU Easy and Fun(tm). The [addressing modes](https://bitbucket.org/drewwalton19216801/butterfly-cs/src/dev/Sharp6502/AddressingModes.cs) are implemented in a similar way.

## Memory
The Sharp6502 library included with Butterfly uses a flat 64KB memory buffer, with support for hooking reads and writes at various addresses. This is useful for implementing other devices that need to read/write in the 6502s 16-bit address space. See [Sharp6502/Memory.cs](https://bitbucket.org/drewwalton19216801/butterfly-cs/src/dev/Sharp6502/Memory.cs) for more details on how that works.

## Building
Run `dotnet publish` from the root repository directory to build the Butterfly emulator and supporting libraries. [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) and [Raylib-CsLo](https://github.com/NotNotTech/Raylib-CsLo) provide the libraries used for the Monitor interface as well as video output.

## Running
TODO: Currently, the emulator will automatically load `rom.bin` in the same directory as the ButterflyCS executable.

## Contributing
Your contributions are welcome. Please feel free to fork this project and submit pull requests.

## More Documentation
TODO: Write documentation.