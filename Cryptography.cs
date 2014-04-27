using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;

namespace MineLib.Network
{
    //Thanks to SirCmpwn!
    public static class Cryptography
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
                p[i] = (byte) ~p[i];
                if (carry)
                {
                    carry = p[i] == 0xFF;
                    p[i]++;
                }
            }
            return p;
        }
    }

    // From https://github.com/ags131/SharpMinecraftLibrary
    public class AsnKeyParser
    {
        private readonly AsnParser _parser;

        public AsnKeyParser(byte[] data)
        {
            _parser = new AsnParser(data);
        }

        private static byte[] TrimLeadingZero(byte[] values)
        {
            byte[] r;
            if ((0x00 == values[0]) && (values.Length > 1))
            {
                r = new byte[values.Length - 1];
                Array.Copy(values, 1, r, 0, values.Length - 1);
            }
            else
            {
                r = new byte[values.Length];
                Array.Copy(values, r, values.Length);
            }

            return r;
        }

        private static bool EqualOid(byte[] first, byte[] second)
        {
            if (first.Length != second.Length)
                return false;
            

            for (var i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                    return false;
                
            }

            return true;
        }

        public RSAParameters ParseRSAPublicKey()
        {
            var parameters = new RSAParameters();

            // Checkpoint
            var position = _parser.CurrentPosition();

            // Ignore Sequence - PublicKeyInfo
            var length = _parser.NextSequence();
            if (length != _parser.RemainingBytes())
            {
                var sb = new StringBuilder("Incorrect Sequence Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                    length.ToString(CultureInfo.InvariantCulture),
                    _parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = _parser.CurrentPosition();

            // Ignore Sequence - AlgorithmIdentifier
            length = _parser.NextSequence();
            if (length > _parser.RemainingBytes())
            {
                var sb = new StringBuilder("Incorrect AlgorithmIdentifier Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                    length.ToString(CultureInfo.InvariantCulture),
                    _parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = _parser.CurrentPosition();
            // Grab the OID
            var value = _parser.NextOID();
            byte[] oid = {0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01};
            if (!EqualOid(value, oid))
                throw new BerDecodeException("Expected OID 1.2.840.113549.1.1.1", position);
            

            // Optional Parameters
            if (_parser.IsNextNull())
                _parser.NextNull(); 
                // Also OK: value = parser.Next();
            
            else
                // Gracefully skip the optional data
                _parser.Next();
            

            // Checkpoint
            position = _parser.CurrentPosition();

            // Ignore BitString - PublicKey
            length = _parser.NextBitString();
            if (length > _parser.RemainingBytes())
            {
                var sb = new StringBuilder("Incorrect PublicKey Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                    length.ToString(CultureInfo.InvariantCulture),
                    (_parser.RemainingBytes()).ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = _parser.CurrentPosition();

            // Ignore Sequence - RSAPublicKey
            length = _parser.NextSequence();
            if (length < _parser.RemainingBytes())
            {
                var sb = new StringBuilder("Incorrect RSAPublicKey Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                    length.ToString(CultureInfo.InvariantCulture),
                    _parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            parameters.Modulus = TrimLeadingZero(_parser.NextInteger());
            parameters.Exponent = TrimLeadingZero(_parser.NextInteger());

            Debug.Assert(0 == _parser.RemainingBytes());

            return parameters;
        }
    }

    internal class AsnParser
    {
        private readonly int _initialCount;
        private readonly List<byte> _octets;

        internal AsnParser(byte[] values)
        {
            _octets = new List<byte>(values.Length);
            _octets.AddRange(values);

            _initialCount = _octets.Count;
        }

        internal int CurrentPosition()
        {
            return _initialCount - _octets.Count;
        }

        internal int RemainingBytes()
        {
            return _octets.Count;
        }

        private int GetLength()
        {
            var length = 0;

            // Checkpoint
            var position = CurrentPosition();

            try
            {
                var b = GetNextOctet();

                if (b == (b & 0x7f))
                    return b;

                var i = b & 0x7f;

                if (i > 4)
                {
                    var sb = new StringBuilder("Invalid Length Encoding. ");
                    sb.AppendFormat("Length uses {0} octets",
                        i.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                while (0 != i--)
                {
                    // shift left
                    length <<= 8;

                    length |= GetNextOctet();
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new BerDecodeException("Error Parsing Key", position, ex);
            }

            return length;
        }

        internal byte[] Next()
        {
            var position = CurrentPosition();

            try
            {
                GetNextOctet();

                var length = GetLength();
                if (length > RemainingBytes())
                {
                    var sb = new StringBuilder("Incorrect Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                        length.ToString(CultureInfo.InvariantCulture),
                        RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return GetOctets(length);
            }

            catch (ArgumentOutOfRangeException ex)
            {
                throw new BerDecodeException("Error Parsing Key", position, ex);
            }
        }

        private byte GetNextOctet()
        {
            var position = CurrentPosition();

            if (0 == RemainingBytes())
            {
                var sb = new StringBuilder("Incorrect Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                    1.ToString(CultureInfo.InvariantCulture),
                    RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            var b = GetOctets(1)[0];

            return b;
        }

        private byte[] GetOctets(int octetCount)
        {
            var position = CurrentPosition();

            if (octetCount > RemainingBytes())
            {
                var sb = new StringBuilder("Incorrect Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                    octetCount.ToString(CultureInfo.InvariantCulture),
                    RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            var values = new byte[octetCount];

            try
            {
                _octets.CopyTo(0, values, 0, octetCount);
                _octets.RemoveRange(0, octetCount);
            }

            catch (ArgumentOutOfRangeException ex)
            {
                throw new BerDecodeException("Error Parsing Key", position, ex);
            }

            return values;
        }

        internal bool IsNextNull()
        {
            return 0x05 == _octets[0];
        }

        internal int NextNull()
        {
            var position = CurrentPosition();

            try
            {
                var b = GetNextOctet();
                if (0x05 != b)
                {
                    var sb = new StringBuilder("Expected Null. ");
                    sb.AppendFormat("Specified Identifier: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                // Next octet must be 0
                b = GetNextOctet();
                if (0x00 != b)
                {
                    var sb = new StringBuilder("Null has non-zero size. ");
                    sb.AppendFormat("Size: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return 0;
            }

            catch (ArgumentOutOfRangeException ex)
            {
                throw new BerDecodeException("Error Parsing Key", position, ex);
            }
        }

        internal bool IsNextSequence()
        {
            return 0x30 == _octets[0];
        }

        internal int NextSequence()
        {
            var position = CurrentPosition();

            try
            {
                var b = GetNextOctet();
                if (0x30 != b)
                {
                    var sb = new StringBuilder("Expected Sequence. ");
                    sb.AppendFormat("Specified Identifier: {0}",
                        b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                var length = GetLength();
                if (length > RemainingBytes())
                {
                    var sb = new StringBuilder("Incorrect Sequence Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                        length.ToString(CultureInfo.InvariantCulture),
                        RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return length;
            }

            catch (ArgumentOutOfRangeException ex)
            {
                throw new BerDecodeException("Error Parsing Key", position, ex);
            }
        }

        internal bool IsNextOctetString()
        {
            return 0x04 == _octets[0];
        }

        internal int NextOctetString()
        {
            var position = CurrentPosition();

            try
            {
                var b = GetNextOctet();
                if (0x04 != b)
                {
                    var sb = new StringBuilder("Expected Octet String. ");
                    sb.AppendFormat("Specified Identifier: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                var length = GetLength();
                if (length > RemainingBytes())
                {
                    var sb = new StringBuilder("Incorrect Octet String Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                        length.ToString(CultureInfo.InvariantCulture),
                        RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return length;
            }

            catch (ArgumentOutOfRangeException ex)
            {
                throw new BerDecodeException("Error Parsing Key", position, ex);
            }
        }

        internal bool IsNextBitString()
        {
            return 0x03 == _octets[0];
        }

        internal int NextBitString()
        {
            var position = CurrentPosition();

            try
            {
                var b = GetNextOctet();
                if (0x03 != b)
                {
                    var sb = new StringBuilder("Expected Bit String. ");
                    sb.AppendFormat("Specified Identifier: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                var length = GetLength();

                // We need to consume unused bits, which is the first
                //   octet of the remain values
                b = _octets[0];
                _octets.RemoveAt(0);
                length--;

                if (0x00 != b)
                    throw new BerDecodeException("The first octet of BitString must be 0", position);
                

                return length;
            }

            catch (ArgumentOutOfRangeException ex)
            {
                throw new BerDecodeException("Error Parsing Key", position, ex);
            }
        }

        internal bool IsNextInteger()
        {
            return 0x02 == _octets[0];
        }

        internal byte[] NextInteger()
        {
            var position = CurrentPosition();

            try
            {
                var b = GetNextOctet();
                if (0x02 != b)
                {
                    var sb = new StringBuilder("Expected Integer. ");
                    sb.AppendFormat("Specified Identifier: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                var length = GetLength();
                if (length > RemainingBytes())
                {
                    var sb = new StringBuilder("Incorrect Integer Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                        length.ToString(CultureInfo.InvariantCulture),
                        RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return GetOctets(length);
            }

            catch (ArgumentOutOfRangeException ex)
            {
                throw new BerDecodeException("Error Parsing Key", position, ex);
            }
        }

        internal byte[] NextOID()
        {
            var position = CurrentPosition();

            try
            {
                var b = GetNextOctet();
                if (0x06 != b)
                {
                    var sb = new StringBuilder("Expected Object Identifier. ");
                    sb.AppendFormat("Specified Identifier: {0}",
                        b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                var length = GetLength();
                if (length > RemainingBytes())
                {
                    var sb = new StringBuilder("Incorrect Object Identifier Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                        length.ToString(CultureInfo.InvariantCulture),
                        RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                var values = new byte[length];

                for (var i = 0; i < length; i++)
                {
                    values[i] = _octets[0];
                    _octets.RemoveAt(0);
                }

                return values;
            }

            catch (ArgumentOutOfRangeException ex)
            {
                throw new BerDecodeException("Error Parsing Key", position, ex);
            }
        }
    }

    [Serializable]
    internal sealed class BerDecodeException : Exception
    {
        private readonly int _mPosition;

        public BerDecodeException()
        {
        }

        public BerDecodeException(String message)
            : base(message)
        {
        }

        public BerDecodeException(String message, Exception ex)
            : base(message, ex)
        {
        }

        public BerDecodeException(String message, int position)
            : base(message)
        {
            _mPosition = position;
        }

        public BerDecodeException(String message, int position, Exception ex)
            : base(message, ex)
        {
            _mPosition = position;
        }

        private BerDecodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _mPosition = info.GetInt32("Position");
        }

        public int Position
        {
            get { return _mPosition; }
        }

        public override string Message
        {
            get
            {
                var sb = new StringBuilder(base.Message);

                sb.AppendFormat(" (Position {0}){1}",
                    _mPosition, Environment.NewLine);

                return sb.ToString();
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Position", _mPosition);
        }
    }
}