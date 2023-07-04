
//------------------------------------------------------------------------------

//  <auto-generated>
//      This code was generated by:
//        TerminalGuiDesigner v1.0.24.0
//      You can make changes to this file and they will not be overwritten when saving.
//  </auto-generated>
// -----------------------------------------------------------------------------
namespace ButterflyCS.Monitor.Wizards {
    using Terminal.Gui;
    
    
    public partial class OpenFile {
        private string filePath = string.Empty; // The file path
        private ushort loadAddress = 0x0000; // The load address

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFile"/> class.
        /// </summary>
        public OpenFile() {
            InitializeComponent();

            btnLoad.Clicked += () => btnLoad_Clicked();
            btnCancel.Clicked += () => btnCancel_Clicked();

            txtFilePath.KeyPress += (keyEvent) =>
            {
                if (keyEvent.KeyEvent.Key == Terminal.Gui.Key.Enter)
                {
                    CheckAndLoadFile();
                }
            };

            txtLoadAddress.KeyPress += (keyEvent) =>
            {
                if (keyEvent.KeyEvent.Key == Terminal.Gui.Key.Enter)
                {
                    CheckAndLoadFile();
                }
            };
        }

        private void btnLoad_Clicked()
        {
            CheckAndLoadFile();
        }

        private void btnCancel_Clicked()
        {
            Application.RequestStop();
        }

        private void CheckAndLoadFile()
        {
            // Get the file path
            filePath = (string)txtFilePath.Text;

            // Get the address
            string address = (string)txtLoadAddress.Text;

            // Is the file path valid?
            if (File.Exists(filePath))
            {
                // If the address is empty, show a dialog telling the user to enter an address
                if (address.Length == 0)
                {
                    MessageBox.ErrorQuery(50, 7, "Error", "Please enter an address", "OK");
                    return;
                }

                // Get the address
                loadAddress = Convert.ToUInt16(address, 16);

                // Load the file
                Machine.LoadProgram(filePath, loadAddress);

                // Add the output to the output text view
                MonitorOutput.Add("Loaded file " + filePath + " at address " + loadAddress.ToString("X4"));

                // Close the window
                Application.RequestStop();
            }
            else
            {
                // Show a dialog telling the user that the file path is invalid
                MessageBox.ErrorQuery(50, 7, "Error", "Invalid file path", "OK");
            }
        }
    }
}
