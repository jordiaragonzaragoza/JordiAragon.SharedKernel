namespace JordiAragon.SharedKernel.Helpers
{
    using System;
    using System.Linq;

    public static class ConcurrencyVersionHelper
    {
        public static void IncrementByteVersion(byte[] version)
        {
            for (int i = version.Length - 1; i >= 0; i--)
            {
                // Increment the current byte and check for overflow
                if (++version[i] != 0)
                {
                    return; // If there's no overflow, exit the loop
                }

                // If the current byte overflows, reset it to zero and continue with the next byte
                version[i] = 0;
            }

            // If all bytes overflow, handle it somehow
            // For example, you could throw an exception or truncate additional bytes
            throw new OverflowException("The counter has reached its maximum value");
        }

        public static byte[] InitializeMinusOneByteVersion()
        {
            return Enumerable.Repeat((byte)255, 8).ToArray();
        }
    }
}