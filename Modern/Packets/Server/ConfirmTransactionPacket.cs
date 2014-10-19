using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public byte WindowId;
        public short ActionNumber;
        public bool Accepted;

        public byte ID { get { return 0x32; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            WindowId = reader.ReadByte();
            ActionNumber = reader.ReadShort();
            Accepted = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowId);
            stream.WriteShort(ActionNumber);
            stream.WriteBoolean(Accepted);
            stream.Purge();

            return this;
        }
    }
}