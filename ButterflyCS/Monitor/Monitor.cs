using ButterflyCS.Monitor.Command;
using Terminal.Gui;

namespace ButterflyCS.Monitor
{
    /// <summary>
    /// The monitor.
    /// </summary>
    public class Monitor : Window
    {
        private readonly Machine _machine; // The machine that this monitor is attached to
        private Interpreter _interpreter; // The interpreter for this monitor

        /// <summary>
        /// Initializes a new instance of the <see cref="Monitor"/> class.
        /// </summary>
        /// <param name="machine">The machine.</param>
        /// <remarks>
        /// This is the main window of the application.
        /// </remarks>
        public Monitor(Machine machine)
        {
            _machine = machine;
            _interpreter = new(_machine);

            // Set the title of the window
            Title = "ButterflyCS Monitor";

            // Set up the output frame
            View outputFrame = new()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            // Set up the output text view
            TextView outputText = new()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ReadOnly = true,
            };
            
            // Add the output text view to the output frame
            outputFrame.Add(outputText);

            // The input box is on the bottom of the window
            var inputBox = new TextField(">")
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
                // Check for delete key
                if ((keyEvent.KeyEvent.Key == Key.DeleteChar) | (keyEvent.KeyEvent.Key == Key.Backspace))
                {
                    // If all we have is the prompt, don't delete it
                    if (inputBox.Text.Length == 1)
                    {
                        keyEvent.Handled = true;
                    }
                }

                if (keyEvent.KeyEvent.Key == Key.Enter)
                {
                    // Strip the prompt (>) from the input
                    string input = (string)inputBox.Text;
                    input = input[1..];

                    // Run the command
                    string? output = _interpreter.InterpretCommand(input);

                    // Add the output to the output text view
                    outputText.Text += output + "\n";

                    // Reset the prompt
                    inputBox.Text = ">";
                    inputBox.CursorPosition = 1;
                }
            };

            Add(outputFrame, inputBox);
        }
    }
}