using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButterflyCS.Monitor
{
    /// <summary>
    /// A global history of the output.
    /// </summary>
    public static class MonitorOutput
    {
        private static List<string> OutputHistory = new();

        /// <summary>
        /// Adds the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        public static void Add(string output)
        {
            OutputHistory.Add(output);
        }

        /// <summary>
        /// Deletes the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public static void Delete(int index)
        {
            OutputHistory.RemoveAt(index);
        }

        /// <summary>
        /// Gets the most recent output.
        /// </summary>
        /// <returns></returns>
        public static string GetMostRecentOutput()
        {
            string output = string.Empty;
            // Get the most recent output, only if there is any output
            if (OutputHistory.Count != 0)
            {
                output = OutputHistory[^1];
            }

            return output;
        }
    }
}
