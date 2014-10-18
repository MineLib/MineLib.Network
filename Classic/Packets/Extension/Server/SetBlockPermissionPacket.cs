﻿using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct SetBlockPermissionPacket : IPacketWithSize
    {
        public byte BlockType;
        public AllowPlacement AllowPlacement;
        public AllowDeletion AllowDeletion;

        public byte ID { get { return 0x1C; } }
        public short Size { get { return 4; } }

        public void ReadPacket(PacketByteReader stream)
        {
            BlockType = stream.ReadByte();
            AllowPlacement = (AllowPlacement) stream.ReadByte();
            AllowDeletion = (AllowDeletion) stream.ReadByte();;
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(BlockType);
            stream.WriteByte((byte) AllowPlacement);
            stream.WriteByte((byte) AllowDeletion);
            stream.Purge();
        }
    }
}