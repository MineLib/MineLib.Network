﻿using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct ExtRemovePlayerNamePacket : IPacketWithSize
    {
        public short NameID;

        public byte ID { get { return 0x18; } }
        public short Size { get { return 3; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            NameID = stream.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(NameID);
            stream.Purge();

            return this;
        }
    }
}
