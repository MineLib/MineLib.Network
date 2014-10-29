using System;
using Org.BouncyCastle.Math;

namespace MineLib.Network.IO
{
    /// <summary>
    /// Object that reads VarInt (or Byte) and ByteArray for handling Data later 
    /// and writes any data from packet to user-defined object, that will interact with Minecraft Server.
    /// </summary>
    public interface IMinecraftStream : IDisposable
    {
        NetworkMode Mode { get; set; }

        // -- Modern
        bool EncryptionEnabled { get; set; }

        bool ModernCompressionEnabled { get; set; }
        long ModernCompressionThreshold { get; set; }

        void InitializeEncryption(byte[] key);

        void SetCompression(long threshold);
        // -- Modern

        void WriteString(string value);

        void WriteVarInt(int value);

        void WriteBoolean(bool value);

        void WriteSByte(sbyte value);
        void WriteByte(byte value);

        void WriteShort(short value);
        void WriteUShort(ushort value);

        void WriteInt(int value);
        void WriteUInt(uint value);

        void WriteLong(long value);
        void WriteULong(ulong value);

        void WriteBigInteger(BigInteger value);
        void WriteUBigInteger(BigInteger value);

        void WriteDouble(double value);

        void WriteFloat(float value);


        void WriteStringArray(string[] value);

        void WriteVarIntArray(int[] value);

        void WriteIntArray(int[] value);

        void WriteByteArray(byte[] value);


        byte ReadByte();

        int ReadVarInt();

        byte[] ReadByteArray(int value);


        IAsyncResult BeginWrite(IPacket packet, AsyncCallback callback, object state);
        void EndWrite(IAsyncResult asyncResult);

        IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state);

        int EndRead(IAsyncResult asyncResult);


        void Purge();
    }
}