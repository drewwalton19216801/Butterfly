using Raylib_CsLo;
using Sharp6502;

namespace ButterflyCS.GUI
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
        /// Draw the disassembly?
        /// </summary>
        /// <remarks>
        /// This is a toggleable option and is used to determine 
        /// whether to draw the disassembly or not in the memory view.
        /// </remarks>
        private ushort disasmAddress = 0x8000;

        /// <summary>
        /// The disassembly string.
        /// </summary>
        private string disasmString = "";

        /// <summary>
        /// The gamepad.
        /// </summary>
        private readonly int gamepad = 0;

        /// <summary>
        /// The emulator screen.
        /// </summary>
        public enum EmulatorScreen
        {
            /// <summary>
            /// The machine state screen.
            /// </summary>
            MachineState,

            /// <summary>
            /// The memory view screen.
            /// </summary>
            Memory,

            /// <summary>
            /// The disassembly screen.
            /// </summary>
            Disassembly,

            /// <summary>
            /// The stack screen.
            /// </summary>
            Stack,

            /// <summary>
            /// The flags screen.
            /// </summary>
            Flags,

            /// <summary>
            /// The registers screen.
            /// </summary>
            Registers,

            /// <summary>
            /// The breakpoints screen.
            /// </summary>
            Breakpoints,

            /// <summary>
            /// The help screen.
            /// </summary>
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
        public void ShowWindow()
        {
            // Disable Raylib logging, we'll eventually redirect it to our own logging system
            Raylib.SetTraceLogLevel((int)TraceLogLevel.LOG_NONE);

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

            // Check for pause (Ctrl+P or gamepad Start)
            if ((Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_P)) || Raylib.IsGamepadButtonPressed(gamepad, GamepadButton.GAMEPAD_BUTTON_MIDDLE_RIGHT))
            {
                machine.isPaused = !machine.isPaused;
                machine.isSingleStepping = false;
            }
            // Check for single-step (Ctrl+S or gamepad Back)
            else if ((Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_S)) || Raylib.IsGamepadButtonPressed(gamepad, GamepadButton.GAMEPAD_BUTTON_MIDDLE_LEFT))
            {
                machine.isSingleStepping = true;
                machine.isPaused = true;
            }

            // If machine is in single-step mode, execute one cycle if the user presses the spacebar or gamepad A
            if (machine.isSingleStepping && (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsGamepadButtonPressed(gamepad, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_RIGHT)))
            {
                Log.Debug(subsystem, "Single-stepping");
                machine.cpu.Clock();
            }

            // Each screen can have its own input processing
            switch (currentScreen)
            {
                case EmulatorScreen.MachineState:
                    {
                        // Check for Ctrl+PgUp or Gamepad Up
                        if ((Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_UP)) || Raylib.IsGamepadButtonPressed(gamepad, GamepadButton.GAMEPAD_BUTTON_LEFT_FACE_UP))
                        {
                            // Increase the speed by 2 Hz
                            machine.IncreaseSpeed(2);
                        }

                        // Check for Ctrl+PgDown or Gamepad Down
                        if ((Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_DOWN)) || Raylib.IsGamepadButtonPressed(gamepad, GamepadButton.GAMEPAD_BUTTON_LEFT_FACE_DOWN))
                        {
                            // Decrease the speed by 2 Hz
                            machine.DecreaseSpeed(2);
                        }

                        // Check for Ctrl+N
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_N))
                        {
                            // Toggle NMI
                            machine.cpu.NMI();
                        }

                        // Check for Ctrl+I
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_I))
                        {
                            // Toggle IRQ
                            machine.cpu.IRQ();
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

                        // Check for Up
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP) || Raylib.IsGamepadButtonPressed(gamepad, GamepadButton.GAMEPAD_BUTTON_LEFT_FACE_UP))
                        {
                            // Increase the memory view start address by 1 address,
                            // but don't go past the end of memory (0xFFFF)
                            memoryViewStartAddress = (ushort)Math.Min(memoryViewStartAddress + 1, 0xFFFF);
                        }

                        // Check for Down
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) || Raylib.IsGamepadButtonPressed(gamepad, GamepadButton.GAMEPAD_BUTTON_LEFT_FACE_DOWN))
                        {
                            // Decrease the memory view start address by 1 address,
                            // but don't go past the start of memory (0x0000)
                            memoryViewStartAddress = (ushort)Math.Max(memoryViewStartAddress - 1, 0x0000);
                        }

                        // Check for Enter (Return) or Gamepad B
                        // This will allow the user to disassemble the currently highlighted address
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER) || Raylib.IsGamepadButtonPressed(gamepad, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_DOWN))
                        {
                            disasmAddress = (ushort)(memoryViewStartAddress + 4);
                            UpdateDisassembly();
                        }
                        break;
                    }
            }

            // Check for reset (Ctrl+R or gamepad Y)
            if ((Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_R)) || Raylib.IsGamepadButtonPressed(gamepad, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_LEFT))
            {
                // Reset the machine
                machine.Reset();
            }

            // Check for quit (Ctrl+Q)
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
            {
                // Quit the application
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

            byte regA = 0;
            byte regX = 0;
            byte regY = 0;
            byte regSP = 0;
            ushort regPC = 0;
            string currentDisassembly = string.Empty;
            byte mem6000 = 0;
            byte mem6002 = 0;
            string variantString = string.Empty;

            // Lock the cpu lock object while we read its state
            lock (machine.cpu.cpuLock)
            {
                regA = machine.cpu.registers.A;
                regX = machine.cpu.registers.X;
                regY = machine.cpu.registers.Y;
                regSP = machine.cpu.registers.SP;
                regPC = machine.cpu.registers.PC;
                currentDisassembly = machine.cpu.currentDisassembly;
                mem6000 = machine.cpu.memory.Read(0x6000, true);
                mem6002 = machine.cpu.memory.Read(0x6002, true);
                variantString = machine.cpu.cpuVariant switch
                {
                    CPU.Variant.NMOS_6502 => "NMOS 6502",
                    CPU.Variant.CMOS_65C02 => "WDC 65C02",
                    CPU.Variant.NES_6502 => "Ricoh 2A03 (NES)",
                    _ => "Unknown"
                };
            }

            // Draw the registers (prefixes with 0x to indicate hexadecimal)
            Raylib.DrawText("Registers", 10, 30, 20, Raylib.BLACK);
            Raylib.DrawText($"A: 0x{regA:X2}", 10, 50, 20, Raylib.BLACK);
            Raylib.DrawText($"X: 0x{regX:X2}", 10, 70, 20, Raylib.BLACK);
            Raylib.DrawText($"Y: 0x{regY:X2}", 10, 90, 20, Raylib.BLACK);
            Raylib.DrawText($"SP: 0x{regSP:X2}", 10, 110, 20, Raylib.BLACK);
            Raylib.DrawText($"PC: 0x{regPC:X4}", 10, 130, 20, Raylib.BLACK);

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
            Raylib.DrawText($"Instruction: {currentDisassembly}", 10, 210, 20, Raylib.BLACK);

            // Draw the cycles remaining
            Raylib.DrawText($"Cycles Remaining: {machine.cpu.cycles}", 10, 230, 20, Raylib.BLACK);

            // Draw the paused state
            Raylib.DrawText($"Paused: {machine.isPaused}", 10, 250, 20, Raylib.BLACK);

            // Draw the single-stepping state
            Raylib.DrawText($"Single-Stepping: {machine.isSingleStepping}", 10, 270, 20, Raylib.BLACK);

            // Draw the memory at address 0x6000
            Raylib.DrawText($"Memory at 0x6000: {mem6000:X2}", 10, 290, 20, Raylib.BLACK);

            // Draw the memory at address 0x6000
            Raylib.DrawText($"Memory at 0x6002: {mem6002:X2}", 10, 310, 20, Raylib.BLACK);

            // Draw the CPU variant
            Raylib.DrawText($"CPU Variant: {variantString}", 10, 330, 20, Raylib.BLACK);

            // Get the current gamepad button being pressed
            GamepadButton currentButton = (GamepadButton)Raylib.GetGamepadButtonPressed();
            // Convert the button to a string
            string buttonString = currentButton switch
            {
                GamepadButton.GAMEPAD_BUTTON_LEFT_FACE_UP => "Up",
                GamepadButton.GAMEPAD_BUTTON_LEFT_FACE_RIGHT => "Right",
                GamepadButton.GAMEPAD_BUTTON_LEFT_FACE_DOWN => "Down",
                GamepadButton.GAMEPAD_BUTTON_LEFT_FACE_LEFT => "Left",
                GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_UP => "X",
                GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_RIGHT => "A",
                GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_DOWN => "B",
                GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_LEFT => "Y",
                GamepadButton.GAMEPAD_BUTTON_LEFT_TRIGGER_1 => "Left Shoulder",
                GamepadButton.GAMEPAD_BUTTON_LEFT_TRIGGER_2 => "Left Trigger",
                GamepadButton.GAMEPAD_BUTTON_RIGHT_TRIGGER_1 => "Right Shoulder",
                GamepadButton.GAMEPAD_BUTTON_RIGHT_TRIGGER_2 => "Right Trigger",
                GamepadButton.GAMEPAD_BUTTON_MIDDLE_LEFT => "Middle Left",
                GamepadButton.GAMEPAD_BUTTON_MIDDLE => "Middle",
                GamepadButton.GAMEPAD_BUTTON_MIDDLE_RIGHT => "Middle Right",
                GamepadButton.GAMEPAD_BUTTON_LEFT_THUMB => "Left Thumb",
                GamepadButton.GAMEPAD_BUTTON_RIGHT_THUMB => "Right Thumb",
                _ => "None"
            };

            // Draw the current gamepad button being pressed
            Raylib.DrawText($"Gamepad Button: {buttonString}", 10, 350, 20, Raylib.BLACK);
        }

        /// <summary>
        /// Draws the memory view screen.
        /// </summary>
        private void DrawMemoryViewScreen(bool drawDisassembly = true)
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
            Raylib.DrawText("Address", 10, 30, 20, Raylib.BLACK);

            // Draw the memory addresses, but highlight the current address
            for (int i = 0; i < 20; i++)
            {
                if (memoryViewStartAddress + i == memoryViewStartAddress + 4)
                {
                    Raylib.DrawText($"{memoryViewStartAddress + i:X4}", 10, 50 + (i * 20), 20, Raylib.RED);
                }
                else
                {
                    Raylib.DrawText($"{memoryViewStartAddress + i:X4}", 10, 50 + (i * 20), 20, Raylib.BLACK);
                }
            }

            // Draw the memory values
            Raylib.DrawText("Value", 100, 30, 20, Raylib.BLACK);

            // Draw the memory values
            for (int i = 0; i < 20; i++)
            {
                Raylib.DrawText($"{machine.cpu.memory.Read((ushort)(memoryViewStartAddress + i), true):X2}", 100, 50 + (i * 20), 20, Raylib.BLACK);
            }

            // Draw the disassembly on the same line as the selected memory address if drawDisassembly is true
            if (drawDisassembly)
            {
                Raylib.DrawText("Disassembly", 200, 30, 20, Raylib.BLACK);
                Raylib.DrawText($"{disasmString}", 200, 50 + (4 * 20), 20, Raylib.YELLOW);
            }
        }

        /// <summary>
        /// Updates the disassembly.
        /// </summary>
        private void UpdateDisassembly()
        {                            
            disasmString = machine.cpu.Disassemble(disasmAddress);
            Log.Debug(subsystem, ": Disassembly at 0x" + disasmAddress.ToString("X4") + ": " + disasmString);
        }
    }
}
