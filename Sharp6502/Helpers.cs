using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp6502
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Converts a float (MHz) to an Int64 (Hz).
        /// </summary>
        /// <param name="mhz">Clock speed in MHz (ex: 6.144)</param>
        /// <returns>An Int64.</returns>
        public static Int64 MhzToHz(this float mhz)
        {
            return (Int64)(mhz * 1000000);
        }

        /// <summary>
        /// convertz an Int64 (Hz) to a string (MHz).
        /// </summary>
        /// <param name="hz">The hz.</param>
        /// <returns>A string.</returns>
        public static string HzToMhz(this Int64 hz)
        {
            return $"{(float)hz / 1000000}";
        }
    }
}
