using System;
using Org.BouncyCastle.Math;

namespace MineLib.Network.IO
{
    /// <summary>
    /// Object that reads data from IPacket.
    /// </summary>
    public interface IMinecraftDataReader : IDisposable
    {
        NetworkMode Mode { get; set; }

        string ReadString();

        int ReadVarInt();

        bool ReadBoolean();

        sbyte ReadSByte();
        byte ReadByte();

        short ReadShort();
        ushort ReadUShort();

        int ReadInt();
        uint ReadUInt();

        long ReadLong();
        ulong ReadULong();

        BigInteger ReadBigInteger();
        BigInteger ReadUBigInteger();

        float ReadFloat();

        double ReadDouble();


        string[] ReadStringArray(int value);

        int[] ReadVarIntArray(int value);

        int[] ReadIntArray(int value);

        byte[] ReadByteArray(int value);
    }
}