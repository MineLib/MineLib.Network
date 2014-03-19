using System;
using System.IO;
using System.Text;

namespace MineLib.Network.IO
{
    // Read only decrypted data
    public class PacketByteReader
    {
        private MemoryStream _stream;

        public PacketByteReader(MemoryStream stream)
        {
            _stream = stream;
        }

        public PacketByteReader(byte[] data)
        {
            _stream = new MemoryStream(data);
        }

        public void SetNewMemoryStream(MemoryStream stream)
        {
            _stream = stream;
        }

        public void SetNewData(byte[] data)
        {
            _stream = new MemoryStream(data);
        }

        // -- Strings

        public string ReadString()
        {
            int length = ReadVarInt();
            byte[] stringBytes = ReadByteArray(length);

            return Encoding.UTF8.GetString(stringBytes);
        }

        // -- Shorts

        public short ReadShort()
        {
            byte[] bytes = ReadByteArray(2);
            Array.Reverse(bytes);

            return BitConverter.ToInt16(bytes, 0);
        }

        // -- Integer

        public int ReadInt()
        {
            byte[] bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
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

        // -- Doubles

        public double ReadDouble()
        {
            byte[] bytes = ReadByteArray(8);
            Array.Reverse(bytes);

            return BitConverter.ToDouble(bytes, 0);
        }

        // -- Floats

        public float ReadFloat()
        {
            byte[] bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        // -- Bytes

        public byte ReadByte()
        {
            return ReadSingleByte();
        }

        // -- SByte

        public sbyte ReadSByte()
        {
            try
            {
                return unchecked((sbyte) ReadSingleByte());
            }
            catch
            {
                return 0;
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

        // -- ByteArray

        public byte[] ReadByteArray(int value)
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

        private byte ReadSingleByte()
        {
            return (byte)_stream.ReadByte();
        }

        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();
        }
    }
}
