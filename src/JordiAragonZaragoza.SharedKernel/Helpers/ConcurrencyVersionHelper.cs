namespace JordiAragonZaragoza.SharedKernel.Helpers
{
    using System;
    using System.Linq;

    public static class ConcurrencyVersionHelper
    {
        public static void IncrementByteVersion(byte[] version)
        {
            ArgumentNullException.ThrowIfNull(version, nameof(version));

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

            // If all bytes overflow, just continue (do nothing special)
            ////throw new OverflowException("The counter has reached its maximum value");
        }

        public static byte[] InitializeMinusOneByteVersion()
        {
            return Enumerable.Repeat((byte)255, 8).ToArray();
        }

        public static long ByteArrayToLong(byte[] version)
        {
            ArgumentNullException.ThrowIfNull(version, nameof(version));

            // Make a copy of the array to avoid modifying the original
            byte[] copy = (byte[])version.Clone();

            // If the system is Little Endian, reverse the byte array
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(copy);
            }

            return BitConverter.ToInt64(copy, 0);
        }

        public static byte[] LongToByteArray(long version)
        {
            byte[] bytes = BitConverter.GetBytes(version);

            // If the system is Little Endian, reverse the byte array
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }
    }
}