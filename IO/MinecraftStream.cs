using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Org.BouncyCastle.Math;

namespace MineLib.Network.IO
{
    // -- Credits to umby24 for encryption support, as taken from CWrapped.
    // -- Credits to SirCmpwn for encryption support, as taken from SMProxy.
    // -- All Write methods doesn't write to any stream. It writes to _buffer. Purge write _buffer to any stream.
    public sealed class MinecraftStream : IMinecraftStream
    {
        private delegate IAsyncResult PacketWrite(IPacket packet);
        private PacketWrite _packetWriteDelegate;

        public NetworkMode Mode { get; set; }

        // -- Modern
        public bool EncryptionEnabled { get; set; }

        public bool ModernCompressionEnabled { get; set; }
        public long ModernCompressionThreshold { get; set; }
        // -- Modern

        private Stream _stream;
        private IAesStream _aesStream;
        private byte[] _buffer;
        private readonly Encoding _encoding;

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
            _aesStream = new BouncyCastleAesStream(_stream, key);
        }

        public void SetCompression(long threshold)
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

        // -- VarInt

        public void WriteVarInt(int value)
        {
            WriteByteArray(GetVarIntBytes(value));
        }

        public static byte[] GetVarIntBytes(long value)
        {
            var byteBuffer = new byte[10];
            short pos = 0;

            do
            {
                var byteVal = (byte)(value & 0x7F);
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
        
        // -- Boolean

        public void WriteBoolean(bool value)
        {
            WriteByte(Convert.ToByte(value));
        }

        // -- SByte & Byte

        public void WriteSByte(sbyte value)
        {
            WriteByte(unchecked((byte)value));
        }

        public void WriteByte(byte value)
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

        // -- Short & UShort

        public void WriteShort(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        public void WriteUShort(ushort value)
        {
            WriteByteArray(new byte[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) (value & 0xFF)
            });
        }

        // -- Int & UInt

        public void WriteInt(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        public void WriteUInt(uint value) // Implement
        {
            WriteByteArray(new[]
            {
                (byte)((value & 0xFF000000) >> 24),
                (byte)((value & 0xFF0000) >> 16),
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            });
        }

        // -- Long & ULong

        public void WriteLong(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        public void WriteULong(ulong value)
        {
            WriteByteArray(new[]
            {
                (byte)((value & 0xFF00000000000000) >> 56),
                (byte)((value & 0xFF000000000000) >> 48),
                (byte)((value & 0xFF0000000000) >> 40),
                (byte)((value & 0xFF00000000) >> 32),
                (byte)((value & 0xFF000000) >> 24),
                (byte)((value & 0xFF0000) >> 16),
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            });
        }

        // -- BigInt & UBigInt

        public void WriteBigInteger(BigInteger value)
        {
            var bytes = value.ToByteArray();
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        public void WriteUBigInteger(BigInteger value)
        {
            throw new NotImplementedException();
        }

        // -- Float

        public void WriteFloat(float value)
        {
            var bytes = BitConverter.GetBytes(value);
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

        // -- IntArray

        public void WriteIntArray(int[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                WriteInt(value[i]);            
        }

        // -- ByteArray

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


        // -- Read methods

        public byte ReadByte()
        {
            if (EncryptionEnabled)
                return (byte)_aesStream.ReadByte();
            else
                return (byte)_stream.ReadByte();
        }

        public int ReadVarInt()
        {
            var result = 0;
            var length = 0;

            while (true)
            {
                var current = ReadByte();
                result |= (current & 0x7F) << length++ * 7;

                if (length > 6)
                    throw new InvalidDataException("Invalid varint: Too long.");

                if ((current & 0x80) != 0x80)
                    break;
            }

            return result;
        }

        public byte[] ReadByteArray(long value)
        {
            if (!EncryptionEnabled)
            {
                var result = new byte[value];

                _stream.Read(result, 0, result.Length);
                return result;
            }
            else
            {
                var result = new byte[value];

                _aesStream.Read(result, 0, result.Length);
                return result;
            }
        }


        #region BeginWrite and BeginRead

        public IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException("Use BeginWrite(IPacket packet, AsyncCallback callback, object state)");
        }

        public IAsyncResult BeginWrite(IPacket packet, AsyncCallback callback, object state)
        {
            _packetWriteDelegate = WriteFunction;

            return _packetWriteDelegate.BeginInvoke(packet, callback, state);
        }

        #region BeginWrite

        private IAsyncResult WriteFunction(IPacket packet)
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
                            return BeginWriteWithCompression(data, null, null);
                        else
                            return BeginWriteWithoutCompression(data, null, null);

                    case NetworkMode.Classic:
                        return BeginWriteClassic(data, null, null);
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
                    return _aesStream.BeginWrite(tempBuf2, 0, tempBuf2.Length, callback, state);
                else
                    return _stream.BeginWrite(tempBuf2, 0, tempBuf2.Length, callback, state);
            }
        }

        private IAsyncResult BeginWriteWithoutCompression(byte[] data, AsyncCallback callback, object state)
        {
            // -- We have already in data Packet length and other stuff, just send it.

            if (EncryptionEnabled)
                return _aesStream.BeginWrite(data, 0, data.Length, callback, state);
            else
                return _stream.BeginWrite(data, 0, data.Length, callback, state);
        }

        #endregion

        public void EndWrite(IAsyncResult asyncResult)
        {
            try
            {
                _packetWriteDelegate.EndInvoke(asyncResult);
            }
            catch (Exception) { }
        }


        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (EncryptionEnabled)
                return _aesStream.BeginRead(buffer, offset, count, callback, state);
            else
                return _stream.BeginRead(buffer, offset, count, callback, state);
        }

        public int EndRead(IAsyncResult asyncResult)
        {
            if (EncryptionEnabled)
                return _aesStream.EndRead(asyncResult);
            else
                return _stream.EndRead(asyncResult);
        }

        #endregion


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

            Buffer.BlockCopy(lenBytes, 0, tempBuff, 0, lenBytes.Length);
            Buffer.BlockCopy(_buffer, 0, tempBuff, lenBytes.Length, _buffer.Length);

            if (EncryptionEnabled)
                _aesStream.Write(tempBuff, 0, tempBuff.Length);
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

            Buffer.BlockCopy(packetLengthByteLength, 0, tempBuf, 0, packetLengthByteLength.Length);
            Buffer.BlockCopy(dataLengthByteLength, 0, tempBuf, packetLengthByteLength.Length, dataLengthByteLength.Length);
            Buffer.BlockCopy(data, 0, tempBuf, packetLengthByteLength.Length + dataLengthByteLength.Length, data.Length);

            if (EncryptionEnabled)
                _aesStream.Write(tempBuf, 0, tempBuf.Length);
            else
                _stream.Write(tempBuf, 0, tempBuf.Length);

            _buffer = null;
        }

        #endregion


        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();

            if (_aesStream != null)
                _aesStream.Dispose();

            _buffer = null;
        }
    }
}
