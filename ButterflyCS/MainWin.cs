using System;
using System.Timers;
using Timer = System.Timers.Timer;
using Raylib_CsLo;
using Sharp6502;
using Microsoft.Win32;

namespace ButterflyCS
{
    /// <summary>
    /// The main GUI window.
    /// </summary>
    public class MainWin
    {
        /// <summary>
        /// The subsystem (for logging).
        /// </summary>
        private static readonly string subsystem = "GUI";

        /// <summary>
        /// The machine.
        /// </summary>
        private readonly Machine machine;

        /// <summary>
        /// The memory view start address.
        /// </summary>
        private ushort memoryViewStartAddress = 0x8000;

        /// <summary>
        /// The emulator screen.
        /// </summary>
        public enum EmulatorScreen
        {
            MachineState,
            Memory,
            Disassembly,
            Stack,
            Flags,
            Registers,
            Breakpoints,
            Help
        }

        private EmulatorScreen currentScreen = EmulatorScreen.MachineState;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWin"/> class.
        /// </summary>
        /// <param name="_machine">The _machine.</param>
        public MainWin(Machine _machine)
        {
            Log.Debug(subsystem, "Initializing GUI");
            machine = _machine;
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        public void StartApplication()
        {
            // Initialize the window
            Raylib.InitWindow(800, 600, "Butterfly 6502 Emulator");

            // Set the framerate
            Raylib.SetTargetFPS(60);

            // Set up the machine
            machine.isRunning = false;
            machine.isPaused = true;
            machine.isSingleStepping = false;

            // Start the CPU timer
            machine.cpuTimer.Start();

            // Main loop
            while (!Raylib.WindowShouldClose())
            {
                // Begin GUI drawing
                Raylib.BeginDrawing();

                switch (currentScreen)
                {
                    case EmulatorScreen.MachineState:
                        DrawMachineStateScreen();
                        break;
                    case EmulatorScreen.Memory:
                        DrawMemoryViewScreen();
                        break;
                }

                // End GUI drawing
                Raylib.EndDrawing();

                bool shouldQuit = ProcessInput();

                if (shouldQuit)
                {
                    break;
                }
            }

            // Close the window
            Raylib.CloseWindow();
        }

        /// <summary>
        /// Processes input events.
        /// </summary>
        /// <returns>A bool.</returns>
        private bool ProcessInput()
        {
            /*
             * F1: Machine state, including CPU speed
             * F2: Memory view
             * F3: Stack
             * F4: Flags
             * F5: Registers
             * F6: Breakpoints
             * F7: Help
             */
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_F1))
            {
                currentScreen = EmulatorScreen.MachineState;
            } else if (Raylib.IsKeyPressed(KeyboardKey.KEY_F2))
            {
                currentScreen = EmulatorScreen.Memory;
            } else if (Raylib.IsKeyPressed(KeyboardKey.KEY_F3))
            {
                currentScreen = EmulatorScreen.Stack;
            } else if (Raylib.IsKeyPressed(KeyboardKey.KEY_F4))
            {
                currentScreen = EmulatorScreen.Flags;
            } else if (Raylib.IsKeyPressed(KeyboardKey.KEY_F5))
            {
                currentScreen = EmulatorScreen.Registers;
            } else if (Raylib.IsKeyPressed(KeyboardKey.KEY_F6))
            {
                currentScreen = EmulatorScreen.Breakpoints;
            } else if (Raylib.IsKeyPressed(KeyboardKey.KEY_F7))
            {
                currentScreen = EmulatorScreen.Help;
            }

            // Check for pause (Ctrl+P)
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_P))
            {
                machine.isPaused = !machine.isPaused;
                machine.isSingleStepping = false;
            }
            // Check for single-step (Ctrl+S)
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_S))
            {
                machine.isSingleStepping = true;
                machine.isPaused = true;
            }

            // If machine is in single-step mode, execute one cycle if the user presses the spacebar
            if (machine.isSingleStepping && Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                Log.Debug(subsystem, "Single-stepping");
                machine.cpu.Clock();
            }

            // Each screen can have its own input processing
            switch (currentScreen)
            {
                case EmulatorScreen.MachineState:
                    {
                        // Check for Ctrl+PgUp
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_UP))
                        {
                            // Increase the speed by 2 Hz
                            machine.IncreaseSpeed(2);
                        }

                        // Check for Ctrl+PgDown
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_DOWN))
                        {
                            // Decrease the speed by 2 Hz
                            machine.DecreaseSpeed(2);
                        }
                        break;
                    }
                case EmulatorScreen.Memory:
                    {
                        // Check for Ctrl+PgUp
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_UP))
                        {
                            // Increase the memory view start address by 15 addresses,
                            // but don't go past the end of memory (0xFFFF)
                            memoryViewStartAddress = (ushort)Math.Min(memoryViewStartAddress + 15, 0xFFFF);
                        }

                        // Check for Ctrl+PgDn
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_DOWN))
                        {
                            // Decrease the memory view start address by 15 addresses,
                            // but don't go past the start of memory (0x0000)
                            memoryViewStartAddress = (ushort)Math.Max(memoryViewStartAddress - 15, 0x0000);
                        }

                        // Check for Ctrl+Up
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
                        {
                            // Increase the memory view start address by 1 address,
                            // but don't go past the end of memory (0xFFFF)
                            memoryViewStartAddress = (ushort)Math.Min(memoryViewStartAddress + 1, 0xFFFF);
                        }

                        // Check for Ctrl+Down
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
                        {
                            // Decrease the memory view start address by 1 address,
                            // but don't go past the start of memory (0x0000)
                            memoryViewStartAddress = (ushort)Math.Max(memoryViewStartAddress - 1, 0x0000);
                        }
                        break;
                    }
            }

            // Check for reset (Ctrl+R)
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_R))
            {
                // Reset the machine
                machine.Reset();
            }

            // Check for quit (Ctrl+Q)
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
            {
                // Quit the application
                Log.Info(subsystem, "Quitting application");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draw the machine's state.
        /// </summary>
        private void DrawMachineStateScreen()
        {
            Raylib.ClearBackground(Raylib.GREEN);

            // Draw the registers
            Raylib.DrawText("Registers", 10, 30, 20, Raylib.BLACK);
            Raylib.DrawText($"A: {machine.cpu.registers.A:X2}", 10, 50, 20, Raylib.BLACK);
            Raylib.DrawText($"X: {machine.cpu.registers.X:X2}", 10, 70, 20, Raylib.BLACK);
            Raylib.DrawText($"Y: {machine.cpu.registers.Y:X2}", 10, 90, 20, Raylib.BLACK);
            Raylib.DrawText($"PC: {machine.cpu.registers.PC:X4}", 10, 110, 20, Raylib.BLACK);
            Raylib.DrawText($"SP: {machine.cpu.registers.SP:X2}", 10, 130, 20, Raylib.BLACK);

            string statusString = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                machine.cpu.registers.GetFlag(CPUFlags.Negative) ? 'N' : 'n',
                machine.cpu.registers.GetFlag(CPUFlags.Overflow) ? 'V' : 'v',
                machine.cpu.registers.GetFlag(CPUFlags.Unused) ? 'U' : 'u',
                machine.cpu.registers.GetFlag(CPUFlags.Break) ? 'B' : 'b',
                machine.cpu.registers.GetFlag(CPUFlags.Decimal) ? 'D' : 'd',
                machine.cpu.registers.GetFlag(CPUFlags.InterruptDisable) ? 'I' : 'i',
                machine.cpu.registers.GetFlag(CPUFlags.Zero) ? 'Z' : 'z',
                machine.cpu.registers.GetFlag(CPUFlags.Carry) ? 'C' : 'c');

            Raylib.DrawText($"Status: {statusString}", 10, 150, 20, Raylib.BLACK);

            // Draw the CPU's speed, converting from Hz to MHz (with 6 decimal places)
            Raylib.DrawText($"Speed: {machine.CycleSpeed / 1000000.0:F6} MHz", 10, 170, 20, Raylib.BLACK);

            // Draw the machine's running state (checking machine.isRunning)
            Raylib.DrawText($"Running: {machine.isRunning}", 10, 190, 20, Raylib.BLACK);

            // Draw the current instruction
            Raylib.DrawText($"Instruction: {machine.cpu.currentDisassembly}", 10, 210, 20, Raylib.BLACK);

            // Draw the cycles remaining
            Raylib.DrawText($"Cycles Remaining: {machine.cpu.cycles}", 10, 230, 20, Raylib.BLACK);

            // Draw the paused state
            Raylib.DrawText($"Paused: {machine.isPaused}", 10, 250, 20, Raylib.BLACK);

            // Draw the single-stepping state
            Raylib.DrawText($"Single-Stepping: {machine.isSingleStepping}", 10, 270, 20, Raylib.BLACK);

            // Draw the memory at address 0x0200
            Raylib.DrawText($"Memory at 0x0200: {machine.cpu.memory.Read(0x0200):X2}", 10, 290, 20, Raylib.BLACK);
        }

        /// <summary>
        /// Draws the memory view screen.
        /// </summary>
        private void DrawMemoryViewScreen()
        {
            Raylib.ClearBackground(Raylib.GREEN);

            /*
             * This screen is split into two parts:
             * 
             * The left half is a hex dump of memory, starting at memoryViewStartAddress. It
             * shows 20 addresses at a time. The current address is highlighted in red and will be memoryViewStartAddress + 4.
             * 
             * The right half is a disassembly of the code at the current address. It shows the instruction at the current address.
             */

            // Draw the memory dump
            Raylib.DrawText("Memory", 10, 30, 20, Raylib.BLACK);

            // Draw the memory addresses
            for (int i = 0; i < 20; i++)
            {
                Raylib.DrawText($"{memoryViewStartAddress + i:X4}", 10, 50 + (i * 20), 20, Raylib.BLACK);
            }

            // Draw the memory values
            for (int i = 0; i < 20; i++)
            {
                Raylib.DrawText($"{machine.cpu.memory.Read((ushort)(memoryViewStartAddress + i)):X2}", 70, 50 + (i * 20), 20, Raylib.BLACK);
            }

            // Draw the current address
            Raylib.DrawText(">", 50, 50 + (4 * 20), 20, Raylib.RED);

            // Draw the disassembly
            Raylib.DrawText("Disassembly", 200, 30, 20, Raylib.BLACK);

            // Draw the current disassembly at the current address (memoryViewStartAddress + 4)
            Raylib.DrawText($"{machine.cpu.Disassemble((ushort)(memoryViewStartAddress + 4))}", 200, 50 + (4 * 20), 20, Raylib.BLACK);
        }
    }
}
