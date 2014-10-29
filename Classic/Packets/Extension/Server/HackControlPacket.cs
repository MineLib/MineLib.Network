﻿using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct HackControlPacket : IPacketWithSize
    {
        public Flying Flying;
        public NoClip NoClip;
        public Speeding Speeding;
        public SpawnControl SpawnControl;
        public ThirdPersonView ThirdPersonView;
        public short JumpHeight;

        public byte ID { get { return 0x20; } }
        public short Size { get { return 8; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            Flying = (Flying) stream.ReadByte();
            NoClip = (NoClip) stream.ReadByte();
            Speeding = (Speeding) stream.ReadByte();
            SpawnControl = (SpawnControl) stream.ReadByte();
            ThirdPersonView = (ThirdPersonView) stream.ReadByte();
            JumpHeight = stream.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte((byte) Flying);
            stream.WriteByte((byte) NoClip);
            stream.WriteByte((byte) Speeding);
            stream.WriteByte((byte) SpawnControl);
            stream.WriteByte((byte) ThirdPersonView);
            stream.WriteShort(JumpHeight);
            stream.Purge();

            return this;
        }
    }
}
