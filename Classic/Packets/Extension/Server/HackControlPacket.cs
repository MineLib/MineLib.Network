using MineLib.Network.Classic.Enums;
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

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            Flying = (Flying) reader.ReadByte();
            NoClip = (NoClip) reader.ReadByte();
            Speeding = (Speeding) reader.ReadByte();
            SpawnControl = (SpawnControl) reader.ReadByte();
            ThirdPersonView = (ThirdPersonView) reader.ReadByte();
            JumpHeight = reader.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
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
