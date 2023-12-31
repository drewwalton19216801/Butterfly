﻿using ButterflyCS.Monitor;
using Sharp6502;
using Terminal.Gui;

namespace ButterflyCS
{
    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The subsystem name.
        /// </summary>
        public static readonly string subsystem = "ButterflyCS";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>A Task.</returns>
        public static Task Main(string[] args)
        {
            // Clear the console
            Console.Clear();

            Machine.Init(CPU.Variant.NMOS_6502, 1);

            // Create the threads for the UIs
            Thread terminalThread = new(RunTerminalUI);
            Thread guiThread = new(RunGUI);

            // Start the threads
            terminalThread.Start();
            guiThread.Start();

            // Wait for the threads to finish
            terminalThread.Join();
            guiThread.Join();
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// The terminal UI task.
        /// </summary>
        /// <returns>A Task.</returns>
        public static void RunTerminalUI()
        {
            Application.Init();
            Application.Run(new MonitorWindow());

            // Quit the application
            Application.Shutdown();
            Environment.Exit(0);
        }

        /// <summary>
        /// The main UI task.
        /// </summary>
        /// <returns>A Task.</returns>
        public static void RunGUI()
        {
            // Initialize the GUI
            GUI.MainWin.ShowWindow();

            // If the GUI is closed, quit the application
            Environment.Exit(0);
        }
    }
}