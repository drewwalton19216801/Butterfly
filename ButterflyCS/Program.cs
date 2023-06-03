﻿using Raylib_CsLo;
using Sharp6502;

namespace ButterflyCS
{
    /// <summary>
    /// The program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>A Task.</returns>
        public static Task Main(string[] args)
        {
            // Initialize the machine
            Machine machine = new();
            machine.Reset();

            Raylib.InitWindow(800, 600, "ButterflyCS");
            Raylib.SetTargetFPS(60);
            // Main emulator loop
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.GREEN);
                Raylib.DrawFPS(10, 10);
                Raylib.DrawText("Raylib_CsLo", 10, 30, 20, Raylib.WHITE);
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
            return Task.CompletedTask;
        }
    }
}