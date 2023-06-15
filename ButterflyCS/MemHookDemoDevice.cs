using Sharp6502;

namespace ButterflyCS
{
    /// <summary>
    /// The mem hook demo device.
    /// </summary>
    public class MemHookDemoDevice
    {
        private static readonly string subsystem = "MemHookDemoDevice"; // For logging
        public ushort startAddress = 0x6000; // Start address of the device
        public ushort endAddress = 0x6002; // End address of the device

        private byte[] data = new byte[3]; // The device's data

        /// <summary>
        /// Initializes a new instance of the <see cref="MemHookDemoDevice"/> class.
        /// </summary>
        public MemHookDemoDevice()
        {
            Log.Debug(subsystem, "MemHookDemoDevice created.");
        }

        /// <summary>
        /// Reads a byte from the device.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>A byte.</returns>
        public byte Read(ushort address)
        {
            ushort realAddress = (ushort)(address - startAddress);
            return data[realAddress];
        }

        /// <summary>
        /// Writes a byte to the device.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="data"></param>
        public void Write(ushort address, byte data) {
            ushort realAddress = (ushort)(address - startAddress);
            this.data[realAddress] = data;
        }
    }
}
