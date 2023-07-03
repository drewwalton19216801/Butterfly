using ButterflyCS.Monitor.Command;
using Terminal.Gui;

namespace ButterflyCS.Monitor
{
    /// <summary>
    /// The monitor.
    /// </summary>
    public class Monitor : Window
    {
        private string previousCommand = string.Empty; // The previous command

        /// <summary>
        /// Initializes a new instance of the <see cref="Monitor"/> class.
        /// </summary>
        /// <remarks>
        /// This is the main window of the application.
        /// </remarks>
        public Monitor()
        {
            // Set the title of the window
            Title = "ButterflyCS Monitor";

            // Set up the output frame
            View outputFrame = new()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                CanFocus = false,
            };

            // Set up the output text view
            TextView outputText = new()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ReadOnly = true,
                ColorScheme = Colors.Dialog,
                CanFocus = false,
            };
            
            // Add the output text view to the output frame
            outputFrame.Add(outputText);

            // The input box is on the bottom of the window
            var inputBox = new TextField("")
            {
                X = 0,
                Y = Pos.Bottom(outputFrame) - 1,
                Width = Dim.Fill(),
                Height = 1,
                CursorPosition = 1,
            };

            // Check for enter on the input box
            inputBox.KeyUp += (keyEvent) =>
            {
                if (keyEvent.KeyEvent.Key == Key.Enter)
                {
                    // Check for empty input
                    if (inputBox.Text.Length != 0)
                    {
                        // Get the input
                        string input = (string)inputBox.Text;

                        // Check for empty input
                        if (input.Length != 0)
                        {
                            // Run the command
                            string? output = Interpreter.InterpretCommand(input);

                            // Add the output to the output text view
                            outputText.Text += output + "\n";

                            // Scroll to the bottom of the output text view
                            outputText.MoveEnd();

                            // Set the previous command
                            previousCommand = input;
                        }
                    }

                    // Reset the prompt
                    inputBox.Text = "";
                    inputBox.CursorPosition = 0;
                }

                // Check for up arrow
                if (keyEvent.KeyEvent.Key == Key.CursorUp)
                {
                    // If the previous command is not empty,
                    // set the input box text to the previous command
                    if (previousCommand.Length != 0)
                    {
                        inputBox.Text = previousCommand;
                        inputBox.CursorPosition = previousCommand.Length;
                    }
                }
            };

            Add(outputFrame, inputBox);
        }
    }
}