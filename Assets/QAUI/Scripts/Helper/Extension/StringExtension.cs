using System;

namespace QAUI
{
    internal static class StringExtension
    {
        public static byte[] ToBytes(this string hex)
        {
            if (hex.Length % 2 != 0) throw new ArgumentException("Hex string length must be even.");

            int length = hex.Length / 2;
            byte[] result = new byte[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = hex.Substring(i * 2, 2).ToByte();
            }

            return result;
        }

        private static byte ToByte(this string hex)
        {
            return Convert.ToByte(hex, 16);
        }
    }
}