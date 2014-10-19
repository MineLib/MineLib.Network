using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct CloseWindowPacket : IPacket
    {
        public byte WindowID;

        public byte ID { get { return 0x2E; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            WindowID = reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            stream.Purge();

            return this;
        }
    }
}