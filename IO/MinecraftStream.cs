using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Org.BouncyCastle.Math;

namespace MineLib.Network.IO
{
    // -- Credits to umby24 for encryption support, as taken from CWrapped.
    public sealed partial class MinecraftStream : IDisposable
    {
        // -- Credits to SirCmpwn for encryption support, as taken from SMProxy.
        public NetworkMode Mode { get; set; }
        public bool EncryptionEnabled { get; set; }

        // -- Modern
        public bool ModernCompressionEnabled { get; set; }
        public int ModernCompressionThreshold { get; set; }
        // -- Modern

        private readonly Stream _stream;
        private BouncyAesStream _crypto;
        private byte[] _buffer;
        private readonly Encoding _encoding;

        private bool _disposed;

        public MinecraftStream(Stream stream, NetworkMode mode)
        {
            _stream = stream;
            Mode = mode;

            switch (Mode)
            {
                case NetworkMode.Modern:
                    _encoding = Encoding.UTF8;
                    break;

                case NetworkMode.Classic:
                    _encoding = Encoding.ASCII;
                    break;
            }
        }

        public void InitializeEncryption(byte[] key)
        {
            _crypto = new BouncyAesStream(_stream, key);
        }

        public void SetCompression(int threshold)
        {
            if (threshold == -1)
            {
                ModernCompressionEnabled = false;
                ModernCompressionThreshold = 0;
            }

            ModernCompressionEnabled = true;
            ModernCompressionThreshold = threshold;
        }

        // -- String

        public void WriteString(string value)
        {
            switch (Mode)
            {
                case NetworkMode.Modern:
                    WriteStringModern(value);
                    break;

                case NetworkMode.Classic:
                    WriteStringClassic(value);
                    break;

                case NetworkMode.PocketEdition:
                    WriteStringPocketEdition(value);
                    break;
            }
        }

        private void WriteStringModern(string value)
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
                final[i] = 0x20;

            Buffer.BlockCopy(_encoding.GetBytes(value), 0, final, 0, value.Length);

            WriteByteArray(final);
        }

        private void WriteStringPocketEdition(string value)
        {
            var final = new byte[8];
            for (var i = 0; i < final.Length; i++)
                final[i] = 0x20;      

            Buffer.BlockCopy(_encoding.GetBytes(value), 0, final, 0, value.Length);

            WriteByteArray(final);
        }

        // -- Short

        public void WriteShort(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- UShort

        public void WriteUShort(ushort value)
        {
            WriteByteArray(new byte[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) (value & 0xFF)
            });
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

        // -- Double

        public void WriteDouble(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Float

        public void WriteFloat(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Byte

        public new byte ReadByte()
        {
            if (EncryptionEnabled)
                return (byte)_crypto.ReadByte();
            else
                return (byte)_stream.ReadByte();
        }

        public new void WriteByte(byte value)
        {
            if (_buffer != null)
            {
                var tempBuff = new byte[_buffer.Length + 1];

                Buffer.BlockCopy(_buffer, 0, tempBuff, 0, _buffer.Length);
                tempBuff[_buffer.Length] = value;

                _buffer = tempBuff;
            }
            else
                _buffer = new byte[] { value };
        }

        // -- SByte

        public void WriteSByte(sbyte value)
        {
            WriteByte(unchecked((byte)value));
        }

        // -- Boolean

        public void WriteBoolean(bool value)
        {
            WriteByte(Convert.ToByte(value));
        }

        // -- IntArray

        public void WriteIntArray(int[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                WriteInt(value[i]);            
        }

        // -- StringArray

        public void WriteStringArray(string[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                WriteString(value[i]);          
        }

        // -- VarIntArray

        public void WriteVarIntArray(int[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                WriteVarInt(value[i]);      
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

        #region Purge

        public void Purge()
        {
            switch (Mode)
            {
                case NetworkMode.Modern:
                    if (ModernCompressionEnabled)
                        PurgeModernWithCompression();
                    else
                        PurgeModernWithoutCompression();
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

        private void PurgeModernWithoutCompression()
        {
            var lenBytes = GetVarIntBytes(_buffer.Length);

            var tempBuff = new byte[_buffer.Length + lenBytes.Length];

            Buffer.BlockCopy(lenBytes, 0, tempBuff, 0              , lenBytes.Length);
            Buffer.BlockCopy(_buffer , 0, tempBuff, lenBytes.Length, _buffer.Length);

            if (EncryptionEnabled)
                _crypto.Write(tempBuff, 0, tempBuff.Length);
            else
                _stream.Write(tempBuff, 0, tempBuff.Length);

            _buffer = null;
        }

        private void PurgeModernWithCompression()
        {
            int packetLength = 0; // -- data.Length + GetVarIntBytes(data.Length).Length
            int dataLength = 0; // -- UncompressedData.Length
            var data = _buffer;

            packetLength = _buffer.Length + GetVarIntBytes(_buffer.Length).Length; // -- Get first Packet length

            if (packetLength >= ModernCompressionThreshold) // -- if Packet length > threshold, compress
            {
                using (var outputStream = new MemoryStream())
                using (var inputStream = new DeflaterOutputStream(outputStream, new Deflater(0)))
                {
                    inputStream.Write(_buffer, 0, _buffer.Length);
                    inputStream.Close();

                    data = outputStream.ToArray();
                }

                dataLength = data.Length;
                packetLength = dataLength + GetVarIntBytes(data.Length).Length; // -- Calculate new packet length
            }


            var packetLengthByteLength = GetVarIntBytes(packetLength);
            var dataLengthByteLength = GetVarIntBytes(dataLength);

            var tempBuf = new byte[data.Length + packetLengthByteLength.Length + dataLengthByteLength.Length];

            Buffer.BlockCopy(packetLengthByteLength, 0, tempBuf,                             0                              , packetLengthByteLength.Length);
            Buffer.BlockCopy(dataLengthByteLength  , 0, tempBuf, packetLengthByteLength.Length                              , dataLengthByteLength.Length);
            Buffer.BlockCopy(data                  , 0, tempBuf, packetLengthByteLength.Length + dataLengthByteLength.Length, data.Length);

            if (EncryptionEnabled)
                _crypto.Write(tempBuf, 0, tempBuf.Length);
            else
                _stream.Write(tempBuf, 0, tempBuf.Length);

            _buffer = null;
        }

        #endregion

        #region BeginWrite and BeginRead

        public IAsyncResult BeginWrite(IPacket packet, AsyncCallback callback, object state)
        {
            using (var ms = new MemoryStream())
            using (var stream = new MinecraftStream(ms, Mode))
            {
                packet.WritePacket(stream);
                var data = ms.ToArray();

                switch (Mode)
                {
                    case NetworkMode.Modern:
                        if (ModernCompressionEnabled)
                            return BeginWriteWithCompression(data, callback, state);
                        else
                            return BeginWriteWithoutCompression(data, callback, state);

                    case NetworkMode.Classic:
                        return BeginWriteClassic(data, callback, state);
                }
            }

            return null;
        }

        private IAsyncResult BeginWriteClassic(byte[] data, AsyncCallback callback, object state)
        {
            return _stream.BeginWrite(data, 0, data.Length, callback, state);
        }

        private IAsyncResult BeginWriteWithCompression(byte[] data, AsyncCallback callback, object state)
        {
            int dataLength = 0; // UncompressedData.Length

            // -- data here is raw IPacket with Packet length.
            using (var reader = new MinecraftDataReader(data, Mode))
            {
                var packetLength = reader.ReadVarInt();
                var packetLengthByteLength1 = GetVarIntBytes(packetLength).Length; // -- Getting size of Packet Length

                var tempBuf1 = new byte[data.Length - packetLengthByteLength1];
                Buffer.BlockCopy(data, packetLengthByteLength1, tempBuf1, 0, tempBuf1.Length); // -- Creating data without Packet Length

                packetLength = tempBuf1.Length + GetVarIntBytes(tempBuf1.Length).Length; // -- Get first Packet length

                // -- Handling this data like normal
                if (packetLength >= ModernCompressionThreshold) // if Packet length > threshold, compress
                {
                    using (var outputStream = new MemoryStream())
                    using (var inputStream = new DeflaterOutputStream(outputStream, new Deflater(0)))
                    {
                        inputStream.Write(tempBuf1, 0, tempBuf1.Length);
                        inputStream.Close();

                        tempBuf1 = outputStream.ToArray();
                    }

                    dataLength = tempBuf1.Length;
                    packetLength = dataLength + GetVarIntBytes(tempBuf1.Length).Length; // calculate new packet length
                }


                var packetLengthByteLength = GetVarIntBytes(packetLength);
                var dataLengthByteLength = GetVarIntBytes(dataLength);

                var tempBuf2 = new byte[tempBuf1.Length + packetLengthByteLength.Length + dataLengthByteLength.Length];

                Buffer.BlockCopy(packetLengthByteLength, 0, tempBuf2, 0                                                          , packetLengthByteLength.Length);
                Buffer.BlockCopy(dataLengthByteLength  , 0, tempBuf2, packetLengthByteLength.Length                              , dataLengthByteLength.Length);
                Buffer.BlockCopy(tempBuf1              , 0, tempBuf2, packetLengthByteLength.Length + dataLengthByteLength.Length, tempBuf1.Length);

                if (EncryptionEnabled)
                    return _crypto.BeginWrite(tempBuf2, 0, tempBuf2.Length, callback, state);
                else
                    return _stream.BeginWrite(tempBuf2, 0, tempBuf2.Length, callback, state);
            }
        }

        private IAsyncResult BeginWriteWithoutCompression(byte[] data, AsyncCallback callback, object state)
        {
            // -- We have already in data Packet length and other stuff, just send it.

            if (EncryptionEnabled)
                return _crypto.BeginWrite(data, 0, data.Length, callback, state);
            else
                return _stream.BeginWrite(data, 0, data.Length, callback, state);
        }


        public new IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (EncryptionEnabled)
                return _crypto.BeginRead(buffer, offset, count, callback, state);
            else
                return _stream.BeginRead(buffer, offset, count, callback, state);
        }

        public new int EndRead(IAsyncResult asyncResult)
        {
            if (EncryptionEnabled)
                return _crypto.EndRead(asyncResult);
            else
                return _stream.EndRead(asyncResult);
        }

        #endregion

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private new void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            //base.Dispose(disposing);
            if (disposing)
            {
                if (_stream != null)
                    _stream.Dispose();

                if (_crypto != null)
                    _crypto.Dispose();
            }

            _disposed = true;
        }

        ~MinecraftStream()
        {
            Dispose(false);
        }
    }
}
