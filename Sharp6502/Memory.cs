namespace Sharp6502
{
    /// <summary>
    /// A memory read hook.
    /// </summary>
    public class MemoryReadHook
    {
        public ushort startAddress;
        public ushort endAddress;
        public Func<ushort, byte> readFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryReadHook"/> class.
        /// </summary>
        /// <param name="startAddress">The start address.</param>
        /// <param name="endAddress">The end address.</param>
        /// <param name="readFunc">The read function.</param>
        public MemoryReadHook(ushort startAddress, ushort endAddress, Func<ushort, byte> readFunc)
        {
            this.startAddress = startAddress;
            this.endAddress = endAddress;
            this.readFunc = readFunc;
        }
    }

    /// <summary>
    /// A memory write hook.
    /// </summary>
    public class MemoryWriteHook
    {
        public ushort startAddress;
        public ushort endAddress;
        public Action<ushort, byte> writeFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryWriteHook"/> class.
        /// </summary>
        /// <param name="startAddress">The start address.</param>
        /// <param name="endAddress">The end address.</param>
        /// <param name="writeFunc">The write function.</param>
        public MemoryWriteHook(ushort startAddress, ushort endAddress, Action<ushort, byte> writeFunc)
        {
            this.startAddress = startAddress;
            this.endAddress = endAddress;
            this.writeFunc = writeFunc;
        }
    }

    /// <summary>
    /// A representation of the 6502's memory space.
    /// </summary>
    /// <remarks>
    /// Memory is implemented as a flat 64K memory space. The first 512 bytes
    /// is the zero page, of which the stack occupies the last 256 bytes 
    /// (0x0100 - 0x01FF). Memory locations can be hooked by other components
    /// to provide additional functionality.
    /// </remarks>
    public class Memory
    {
        /// <summary>
        /// The subsystem name.
        /// </summary>
        public static string subsystem = "Memory";

        /// <summary>
        /// The data.
        /// </summary>
        /// <remarks>
        /// The memory space is a flat 64K space, stored on the heap of the host.
        /// </remarks>
        public byte[] data = new byte[0x10000];

        /// <summary>
        /// The list of registered memory read hooks
        /// </summary>
        public List<MemoryReadHook> readHooks = new();

        /// <summary>
        /// The list of registered memory write hooks
        /// </summary>
        public List<MemoryWriteHook> writeHooks = new();

        /// <summary>
        /// Reads a byte from the specified address.
        /// </summary>
        public byte Read(ushort address)
        {
            // Check for read hooks
            foreach (var hook in readHooks)
            {
                if (address >= hook.startAddress && address <= hook.endAddress)
                {
                    // Log the hook
                    string message = $"Read hook called at address {address:X4}";
                    Log.Debug(subsystem, message);

                    // This address is hooked, so call the hook function instead
                    // of reading from main memory
                    return hook.readFunc(address);
                }
            }

            // Log the read
            string msg = $"Read called at address {address:X4}, got data {data[address]:X2}";
            Log.Debug(subsystem, msg);

            // No hooks, so we read from main memory
            return data[address];
        }

        /// <summary>
        /// Writes a byte to the specified address.
        /// </summary>
        public void Write(ushort address, byte value)
        {
            // Write the value to memory before calling any hooks,
            // so debugging tools can see the value in memory.
            data[address] = value;

            // Log the write
            string msg = $"Write called at address {address:X4}, with data {value:X2}";
            Log.Debug(subsystem, msg);

            // Check for write hooks
            foreach (var hook in writeHooks)
            {
                if (address >= hook.startAddress && address <= hook.endAddress)
                {
                    // Log the hook
                    string message = $"Write hook called at address {address:X4}";
                    Log.Info(subsystem, message);

                    // Call the hook
                    hook.writeFunc(address, value);
                    return;
                }
            }
        }

        /// <summary>
        /// Registers a memory read hook.
        /// </summary>
        /// <param name="startAddress">The start address.</param>
        /// <param name="endAddress">The end address.</param>
        /// <param name="readFunc">The read function to be called.</param>
        /// <remarks>
        /// Allows a component to hook a memory address and provide additional
        /// functionality. For example, the PPU could hook memory addresses 0x2000
        /// through 0x2007 to provide access to the PPU registers.
        /// </remarks>
        public void RegisterReadHook(ushort startAddress, ushort endAddress, Func<ushort, byte> readFunc)
        {
            readHooks.Add(new MemoryReadHook(startAddress, endAddress, readFunc));
        }

        /// <summary>
        /// Registers a memory write hook.
        /// </summary>
        /// <param name="startAddress">The start address.</param>
        /// <param name="endAddress">The end address.</param>
        /// <param name="writeFunc">The write function to be called.</param>
        /// <remarks>
        /// Allows a component to hook a memory address and provide additional
        /// functionality. For example, the audio component could hook memory
        /// addresses 0x4000 through 0x4013 to provide access to the audio registers.
        /// </remarks>
        public void RegisterWriteHook(ushort startAddress, ushort endAddress, Action<ushort, byte> writeFunc)
        {
            writeHooks.Add(new MemoryWriteHook(startAddress, endAddress, writeFunc));
        }
    }
}
