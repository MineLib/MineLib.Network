using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Org.BouncyCastle.Math;

namespace MineLib.Network.IO
{
    // -- Credits to umby24 for encryption support, as taken from CWrapped.
    public partial class PacketStream : IDisposable
    {
        // -- Credits to SirCmpwn for encryption support, as taken from SMProxy.
        public readonly NetworkMode Mode;
        public bool EncryptionEnabled;
        public bool CompressionEnabled;
        public int CompressionThreshold;

        private readonly Stream _stream;
        private IAesStream _crypto;
        private byte[] _buffer;
        private readonly Encoding _encoding;

        public PacketStream(Stream stream, NetworkMode mode = NetworkMode.Main)
        {
            _stream = stream;
            Mode = mode;

            switch (Mode)
            {
                case NetworkMode.Main:
                    _encoding = Encoding.UTF8;
                    break;

                case NetworkMode.Classic:
                    _encoding = Encoding.ASCII;
                    break;
            }
        }

        public void InitializeEncryption(byte[] key)
        {
            if (Type.GetType("Mono.Runtime") != null) // -- Running on Mono
                _crypto = new BouncyAesStream(_stream, key);
            else
                _crypto = new NativeAesStream(_stream, key);
        }

        public void SetCompression(int threshold)
        {
            if (threshold == -1)
            {
                CompressionEnabled = false;
                CompressionThreshold = 0;
            }

            CompressionEnabled = true;
            CompressionThreshold = threshold;
        }

        // -- Strings

        public void WriteString(string value)
        {
            switch (Mode)
            {
                case NetworkMode.Main:
                    WriteStringMain(value);
                    break;

                case NetworkMode.Classic:
                    WriteStringClassic(value);
                    break;

                case NetworkMode.PocketEdition:
                    WriteStringPocketEdition(value);
                    break;
            }
        }

        private void WriteStringMain(string value)
        {
            var length = GetVarIntBytes(value.Length);
            var final = new byte[value.Length + length.Length];

            Buffer.BlockCopy(length, 0, final, 0, length.Length);
            Buffer.BlockCopy(_encoding.GetBytes(value), 0, final, length.Length, value.Length);

            WriteByteArray(final);
        }

        private void WriteStringClassic(string value)
        {
            var final = new byte[64];
            for (var i = 0; i < final.Length; i++)
            {
                final[i] = 0x20;
            }

            Buffer.BlockCopy(_encoding.GetBytes(value), 0, final, 0, value.Length);

            WriteByteArray(final);
        }

        private void WriteStringPocketEdition(string value)
        {
            var final = new byte[8];
            for (var i = 0; i < final.Length; i++)
            {
                final[i] = 0x20;
            }

            Buffer.BlockCopy(_encoding.GetBytes(value), 0, final, 0, value.Length);

            WriteByteArray(final);
        }

        // -- Shorts

        public void WriteShort(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- UShort

        public void WriteUShort(ushort value)
        {
            Write(new[]
            {
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            }, 0, 2);
        }

        // -- Int

        public void WriteInt(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- VarInt

        public int ReadVarInt()
        {
            var result = 0;
            var length = 0;

            while (true)
            {
                var current = ReadByte();
                result |= (current & 0x7F) << length++*7;

                if (length > 6)
                    throw new InvalidDataException("Invalid varint: Too long.");

                if ((current & 0x80) != 0x80)
                    break;
            }

            return result;
        }

        public void WriteVarInt(long value)
        {
            WriteByteArray(GetVarIntBytes(value));
        }

        public static byte[] GetVarIntBytes(long value)
        {
            var byteBuffer = new byte[10];
            short pos = 0;

            do
            {
                var byteVal = (byte) (value & 0x7F);
                value >>= 7;

                if (value != 0)
                    byteVal |= 0x80;

                byteBuffer[pos] = byteVal;
                pos += 1;
            } while (value != 0);

            var result = new byte[pos];
            Buffer.BlockCopy(byteBuffer, 0, result, 0, pos);

            return result;
        }

        // -- Long

        public void WriteLong(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- BigInt

        public void WriteBigInteger(BigInteger value)
        {
            var bytes = value.ToByteArray();
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Doubles

        public void WriteDouble(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Floats

        public void WriteFloat(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Bytes

        public new byte ReadByte()
        {
            return ReadSingleByte();
        }

        public new void WriteByte(byte value)
        {
            SendSingleByte(value);

        }

        // -- SByte

        public void WriteSByte(sbyte value)
        {
            SendSingleByte(unchecked((byte) value));

        }

        // -- Boolean

        public void WriteBoolean(bool value)
        {
            SendSingleByte(Convert.ToByte(value));

        }

        // -- IntArray

        public void WriteIntArray(int[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
            {
                WriteInt(value[i]);
            }
        }

        // -- StringArray

        public void WriteStringArray(string[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
            {
                WriteString(value[i]);
            }
        }

        // -- VarIntArray

        public void WriteVarIntArray(int[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
            {
                WriteVarInt(value[i]);
            }
        }

        // -- ByteArray

        public byte[] ReadByteArray(int value)
        {
            if (!EncryptionEnabled)
            {
                var result = new byte[value];
                if (value == 0) return result;
                int n = value;
                while (true)
                {
                    n -= _stream.Read(result, value - n, n);
                    if (n == 0)
                        break;
                }
                return result;
            }
            else
            {
                var result = new byte[value];
                if (value == 0) return result;
                int n = value;
                while (true)
                {
                    n -= _crypto.Read(result, value - n, n);
                    if (n == 0)
                        break;
                }
                return result;
            }
        }

        public void WriteByteArray(byte[] value)
        {
            if (_buffer != null)
            {
                var tempLength = _buffer.Length + value.Length;
                var tempBuff = new byte[tempLength];

                Buffer.BlockCopy(_buffer, 0, tempBuff, 0, _buffer.Length);
                Buffer.BlockCopy(value, 0, tempBuff, _buffer.Length, value.Length);

                _buffer = tempBuff;
            }
            else
                _buffer = value;
        }

        #region Send and Receive

        private byte ReadSingleByte()
        {
            if (EncryptionEnabled)
                return (byte) _crypto.ReadByte();
            return (byte) _stream.ReadByte();
        }

        private void SendSingleByte(byte thisByte)
        {
            if (_buffer != null)
            {
                var tempBuff = new byte[_buffer.Length + 1];

                Buffer.BlockCopy(_buffer, 0, tempBuff, 0, _buffer.Length);
                tempBuff[_buffer.Length] = thisByte;

                _buffer = tempBuff;
            }
            else
                _buffer = new[] {thisByte};
            
        }

        public void Purge()
        {
            switch (Mode)
            {
                case NetworkMode.Main:
                    if (CompressionEnabled)
                        PurgeMainWithCompression();
                    else
                        PurgeMainWithoutCompression();
                    break;

                case NetworkMode.Classic:
                    PurgeClassic();
                    break;
            }
        }

        private void PurgeClassic()
        {
            _stream.Write(_buffer, 0, _buffer.Length);

            _buffer = null;
        }

        private void PurgeMainWithoutCompression()
        {
            var lenBytes = GetVarIntBytes(_buffer.Length);

            var tempBuff = new byte[_buffer.Length + lenBytes.Length];

            Buffer.BlockCopy(lenBytes, 0, tempBuff, 0, lenBytes.Length);
            Buffer.BlockCopy(_buffer, 0, tempBuff, lenBytes.Length, _buffer.Length);

            if (EncryptionEnabled)
                _crypto.Write(tempBuff, 0, tempBuff.Length);
            else
                _stream.Write(tempBuff, 0, tempBuff.Length);

            _buffer = null;
        }

        private void PurgeMainWithCompression()
        {
            int packetLength = 0; // data.Length + GetVarIntBytes(data.Length).Length
            int dataLength = 0; // UncompressedData.Length
            byte[] data = _buffer;

            packetLength = _buffer.Length + GetVarIntBytes(_buffer.Length).Length; // Get first Packet length
            if (CompressionEnabled)
            {
                if (packetLength >= CompressionThreshold) // if Packet length > threshold, compress
                {
                    using (var outputStream = new MemoryStream())
                    using (var inputStream = new DeflaterOutputStream(outputStream, new Deflater(0)))
                    {
                        inputStream.Write(_buffer, 0, _buffer.Length);
                        inputStream.Close();

                        data = outputStream.ToArray();
                    }

                    dataLength = data.Length;
                    packetLength = dataLength + GetVarIntBytes(data.Length).Length; // calculate new packet length
                }
            }

            var lenBytes1 = GetVarIntBytes(packetLength);
            var lenBytes2 = GetVarIntBytes(dataLength);

            var tempBuf = new byte[data.Length + lenBytes1.Length + lenBytes2.Length];

            Buffer.BlockCopy(lenBytes1, 0, tempBuf, 0, lenBytes1.Length);
            Buffer.BlockCopy(lenBytes2, 0, tempBuf, lenBytes1.Length, lenBytes2.Length);

            Buffer.BlockCopy(data, 0, tempBuf, lenBytes1.Length + lenBytes2.Length, data.Length);

            if (EncryptionEnabled)
                _crypto.Write(tempBuf, 0, tempBuf.Length);
            else
                _stream.Write(tempBuf, 0, tempBuf.Length);

            _buffer = null;
        }

        #endregion

        public new void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();

            if (_crypto != null)
                _crypto.Dispose();
        }
    }
}
