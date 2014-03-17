using System;
using System.IO;
using System.Text;

namespace MineLib.Network.IO
{
    // No implementing Stream here. We make a new Wrapped at every packet handling.
    // -- Credits to umby24 for encryption support, as taken from CWrapped.
    public class Wrapped : IDisposable
    {
        // -- Credits to SirCmpwn for encryption support, as taken from SMProxy.
        private readonly Stream _stream;
        private AesStream _crypto;
        public bool EncEnabled;
        private byte[] _buffer;

        public Wrapped(Stream stream)
        {
            _stream = stream;
        }

        public void InitEncryption(byte[] key)
        {
            _crypto = new AesStream(_stream, key);
        }

        // -- Strings

        public string ReadString()
        {
            int length = ReadVarInt();
            byte[] stringBytes = ReadByteArray(length);

            return Encoding.UTF8.GetString(stringBytes);
        }

        public void WriteString(string value)
        {
            byte[] length = GetVarIntBytes((long) value.Length);
            byte[] final = new byte[value.Length + length.Length];

            Buffer.BlockCopy(length, 0, final, 0, length.Length);
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(value), 0, final, length.Length, value.Length);

            WriteByteArray(final);
        }

        // -- Shorts

        public short ReadShort()
        {
            byte[] bytes = ReadByteArray(2);
            Array.Reverse(bytes);

            return BitConverter.ToInt16(bytes, 0);
        }

        public void WriteShort(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Integer

        public int ReadInt()
        {
            byte[] bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

        public void WriteInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- VarInt

        public int ReadVarInt()
        {
            int result = 0;
            int length = 0;

            while (true)
            {
                byte current = ReadByte();
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
            byte[] byteBuffer = new byte[10];
            short pos = 0;

            do
            {
                byte byteVal = (byte) (value & 0x7F);
                value >>= 7;

                if (value != 0)
                    byteVal |= 0x80;

                byteBuffer[pos] = byteVal;
                pos += 1;
            } while (value != 0);

            byte[] result = new byte[pos];
            Buffer.BlockCopy(byteBuffer, 0, result, 0, pos);

            return result;
        }

        // -- Long

        public long ReadLong()
        {
            byte[] bytes = ReadByteArray(8);
            Array.Reverse(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }

        public void WriteLong(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Doubles

        public double ReadDouble()
        {
            byte[] bytes = ReadByteArray(8);
            Array.Reverse(bytes);

            return BitConverter.ToDouble(bytes, 0);
        }

        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Floats

        public float ReadFloat()
        {
            byte[] bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Bytes

        public byte ReadByte()
        {
            return ReadSingleByte();
        }

        public void WriteByte(byte value)
        {
            try
            {
                SendSingleByte(value);
            }
            catch
            {
                return;
            }
        }

        // -- SByte

        public sbyte ReadSByte()
        {
            try
            {
                return Convert.ToSByte(ReadSingleByte());
            }
            catch
            {
                return 0;
            }
        }

        public void WriteSByte(sbyte value)
        {
            try
            {
                SendSingleByte(Convert.ToByte(value));
            }
            catch
            {
                return;
            }
        }

        // -- Bool

        public bool ReadBool()
        {
            try
            {
                return Convert.ToBoolean(ReadSingleByte());
            }
            catch
            {
                return false;
            }
        }

        public void WriteBool(bool value)
        {
            try
            {
                SendSingleByte(Convert.ToByte(value));
            }
            catch
            {
                return;
            }
        }

        // -- IntegerArray

        public int[] ReadIntArray(int value)
        {
            int[] myInts = new int[value];

            for (int i = 0; i < value; i++)
            {
                myInts[i] = ReadInt();
            }

            return myInts;
        }

        public void WriteIntArray(int[] value)
        {
            int length = value.Length;

            for (int i = 0; i < length; i++)
            {
                WriteInt(value[i]);
            }
        }

        // -- StringArray

        public string[] ReadStringArray(int value)
        {
            string[] myStrings = new string[value];

            for (int i = 0; i < value; i++)
            {
                myStrings[i] = ReadString();
            }

            return myStrings;
        }

        public void WriteStringArray(string[] value)
        {
            int length = value.Length;

            for (int i = 0; i < length; i++)
            {
                WriteString(value[i]);
            }
        }

        // -- VarIntArray

        public int[] ReadVarIntArray(int value)
        {
            int[] myInts = new int[value];

            for (int i = 0; i < value; i++)
            {
                myInts[i] = ReadVarInt();
            }

            return myInts;
        }

        public void WriteVarIntArray(int[] value)
        {
            int length = value.Length;

            for (int i = 0; i < length; i++)
            {
                WriteVarInt(value[i]);
            }
        }

        // -- ByteArray

        public byte[] ReadByteArray(int value)
        {
            if (!EncEnabled)
            {
                byte[] myBytes = new byte[value];
                int BytesRead;

                BytesRead = _stream.Read(myBytes, 0, value);

                while (true)
                {
                    if (BytesRead != value)
                    {
                        int newSize = value - BytesRead;
                        int BytesRead1 = _stream.Read(myBytes, BytesRead - 1, newSize);

                        if (BytesRead1 != newSize)
                        {
                            value = newSize;
                            BytesRead = BytesRead1;
                        }
                        else break;
                    }
                    else break;
                }

                return myBytes;
            }
            else
            {
                byte[] myBytes = new byte[value];
                int BytesRead;

                BytesRead = _crypto.DecryptStream.Read(myBytes, 0, value);

                while (true)
                {
                    if (BytesRead != value)
                    {
                        int newSize = value - BytesRead;
                        int BytesRead1 = _crypto.DecryptStream.Read(myBytes, BytesRead - 1, newSize);

                        if (BytesRead1 != newSize)
                        {
                            value = newSize;
                            BytesRead = BytesRead1;
                        }
                        else break;
                    }
                    else break;
                }

                return myBytes;
            }
        }

        public void WriteByteArray(byte[] value)
        {
            if (_buffer != null)
            {
                int tempLength = _buffer.Length + value.Length;
                byte[] tempBuff = new byte[tempLength];

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
            if (EncEnabled)
                return (byte) _crypto.ReadByte();
            else
                return (byte) _stream.ReadByte();
        }

        private void SendSingleByte(byte thisByte)
        {
            if (_buffer != null)
            {
                byte[] tempBuff = new byte[_buffer.Length + 1];

                Buffer.BlockCopy(_buffer, 0, tempBuff, 0, _buffer.Length);
                tempBuff[_buffer.Length] = thisByte;

                _buffer = tempBuff;
            }
            else
            {
                _buffer = new byte[] {thisByte};
            }
        }

        public void Purge()
        {
            byte[] lenBytes = GetVarIntBytes(_buffer.Length);

            byte[] tempBuff = new byte[_buffer.Length + lenBytes.Length];

            Buffer.BlockCopy(lenBytes, 0, tempBuff, 0, lenBytes.Length);
            Buffer.BlockCopy(_buffer, 0, tempBuff, lenBytes.Length, _buffer.Length);

            if (EncEnabled)
                _crypto.EncryptStream.Write(tempBuff, 0, tempBuff.Length);
            else
                _stream.Write(tempBuff, 0, tempBuff.Length);

            _buffer = null;
        }

        #endregion

        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();

            if (_crypto != null)
                _crypto.Dispose();
        }
    }
}
