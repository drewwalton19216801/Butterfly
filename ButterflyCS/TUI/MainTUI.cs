using Terminal.Gui;

namespace ButterflyCS.TUI
{
    /// <summary>
    /// The monitor.
    /// </summary>
    public class Monitor : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Monitor"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="frame">The frame.</param>
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
                Height = Dim.Fill()
            };

            // The input box is on the bottom of the window
            var inputBox = new TextField("")
            {
                X = 0,
                Y = Pos.Bottom(outputFrame) - 1,
                Width = Dim.Fill(),
                Height = 1,
            };

            // Check for enter on the input box
            inputBox.KeyUp += (keyEvent) =>
            {
                if (keyEvent.KeyEvent.Key == Key.Enter)
                {
                    // TODO: Handle input, but for now, just clear the input box
                    inputBox.Text = "";
                }
            };

            Add(outputFrame, inputBox);
        }
    }
}