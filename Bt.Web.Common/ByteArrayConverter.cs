using System;
using System.Linq;

namespace Bt.Web.Common
{
    public static class ByteArrayConverter
    {
        public static string ToHexString(this byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", String.Empty).ToUpperInvariant();
        }

        public static byte[] ToByteArray(this string hex)
        {
            string preparedHex = hex.Trim().ToUpper().Replace(" ", String.Empty).Replace("-", String.Empty);

            if (preparedHex.Length % 2 != 0)
                throw new ArgumentException(String.Format("Hex string cannot have an odd number of digits: {0}", preparedHex));

            return Enumerable.Range(0, preparedHex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(preparedHex.Substring(x, 2), 16))
                .ToArray();
        }
    }
}