using System.Globalization;
using System.Security.Cryptography;

namespace MineLib.Network.Cryptography
{
    //Thanks to SirCmpwn!
    public static class JavaHelper
    {
        /// <summary>
        ///     Produces a Java-style SHA-1 hex digest of the given data.
        /// </summary>
        public static string JavaHexDigest(byte[] data)
        {
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(data);
            var negative = (hash[0] & 0x80) == 0x80;
            if (negative) // check for negative hashes
                hash = TwosCompliment(hash);
            // Create the string and trim away the zeroes
            var digest = GetHexString(hash).TrimStart('0');
            if (negative)
                digest = "-" + digest;
            return digest;
        }

        /// <summary>
        ///     Converts the given n-bit little-endian unsigned number into
        ///     lowercase hexadecimal form.
        /// </summary>
        private static string GetHexString(byte[] p)
        {
            var result = "";
            for (var i = 0; i < p.Length; i++)
                result += p[i].ToString("x2", CultureInfo.InvariantCulture);
            return result;
        }

        /// <summary>
        ///     Given an array that represents an n-bit little-endian signed number,
        ///     the two's compliment (negation) is produced.
        /// </summary>
        private static byte[] TwosCompliment(byte[] p)
        {
            var carry = true;
            for (var i = p.Length - 1; i >= 0; i--)
            {
                p[i] = (byte)~p[i];
                if (carry)
                {
                    carry = p[i] == 0xFF;
                    p[i]++;
                }
            }
            return p;
        }
    }
}